namespace TrueOrFalse
{
    public class GetPersistentLoginCookieValuesResult
    {
        public int UserId = -1;
        public string LoginGuid;

        public bool Exists(){
            return (LoginGuid != null && UserId != -1);
        }
    }
}