using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

public class FacebookUsersController(
    FrontEndUserData _frontEndUserData,
    UserReadingRepo _userReadingRepo,
    SessionUser _sessionUser,
    RegisterUser _registerUser,
    JobQueueRepo _jobQueueRepo,
    UserWritingRepo _userWritingRepo,
    PageViewRepo _pageViewRepo) : ApiBaseController
{
    public readonly record struct LoginRequest(string facebookUserId, string facebookAccessToken);

    public readonly record struct LoginResponse(
        bool Success,
        string MessageKey,
        FrontEndUserData.CurrentUserData Data);

    [HttpPost]
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var user = _userReadingRepo.UserGetByFacebookId(request.facebookUserId);

        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.DoesNotExist
            };
        }

        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(request.facebookAccessToken,
            request.facebookUserId))
        {
            _sessionUser.Login(user, _pageViewRepo);
            _userWritingRepo.Update(user);
            return new LoginResponse
            {
                Success = true,
                Data = _frontEndUserData.Get()
            };
        }

        return new LoginResponse
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        };
    }

    public readonly record struct CreateAndLoginRequest(
        FacebookUserCreateParameter facebookUser,
        string facebookAccessToken,
        string language);

    [HttpPost]
    public async Task<LoginResponse> CreateAndLogin([FromBody] CreateAndLoginRequest request)
    {
        if (await IsFacebookAccessToken.IsAccessTokenValidAsync(request.facebookAccessToken,
            request.facebookUser.id))
        {
            var user = _userReadingRepo.UserGetByFacebookId(request.facebookUser.id);

            if (user != null)
            {
                _sessionUser.Login(user, _pageViewRepo);

                return new LoginResponse
                {
                    Success = true,
                    Data = _frontEndUserData.Get()
                };
            }

            var registerResult = _registerUser.SetFacebookUser(request.facebookUser, request.language);

            if (registerResult.Success)
            {
                return new LoginResponse
                {
                    Success = true,
                    Data = _frontEndUserData.Get()
                };
            }

            return new LoginResponse
            {
                Success = registerResult.Success,
                MessageKey = registerResult.MessageKey
            };
        }

        return new LoginResponse
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.InvalidFBToken
        };
    }

    [HttpGet]
    public bool UserExists([FromBody] string facebookId)
    {
        return _userReadingRepo.FacebookUserExists(facebookId);
    }

    [HttpPost]
    public string DataDeletionCallback(string jsonUserData)
    {
        JObject userData = JObject.Parse(jsonUserData);

        if (String.IsNullOrEmpty(GetHashString(userData["user_id"]?.ToString())))
        {
            return "Error";
        }

        var confirmationCode = GetHashString(userData["user_id"]?.ToString());

        var mailMessage = new MailMessage(
            Settings.EmailFrom,
            Settings.EmailToMemoWikis,
            "Facebook Data Deletion Callback",
            $"The user with the Facebook Id {userData["user_id"]} has made a Facebook data deletion callback. Please delete the Account. Confirmation Code for this Ticket is {confirmationCode}.");

        SendEmail.Run(mailMessage, _jobQueueRepo, _userReadingRepo, MailMessagePriority.High);

        var requestAnswer = new
        {
            url = "http://localhost:26590/FacebookUsersApi/UserExistsString?facebookId=" +
                userData["user_id"],
            confirmation_code = confirmationCode
        };

        return JsonConvert.SerializeObject(requestAnswer);
        ;
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();

        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }

    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
}