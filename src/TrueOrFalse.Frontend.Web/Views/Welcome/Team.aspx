<%@ Page Title="memucho Team" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="team">
        <h2>Das memucho Team</h2>
            <div class="row infoItemRow">
                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_robert201509_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Robert
                    </div>
                    <div class="infoExplanationSnippet">
                        (Gründer)
                    </div>
                </div>

                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_jule201509-2_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Jule
                    </div>
                    <div class="infoExplanationSnippet">
                        (Gründer)
                    </div>
                </div>

                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg" />
                    </div>
                    <div class="infoCatchWord">
                        Christof
                    </div>
                    <div class="infoExplanationSnippet">
                        (Gründer)
                    </div>
                </div>
            </div>    
            <div class="row infoItemRow">
                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="https://ucarecdn.com/6158355b-fff8-4f22-9a04-6a6fc2b6dd61/-/scale_crop/155x155/" />
                    </div>
                    <div class="infoCatchWord">
                        Franziska
                    </div>
                    <div class="infoExplanationSnippet">
                        (Content Managerin)
                    </div>
                </div>
                
                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="https://ucarecdn.com/cbf57b0d-491b-49f0-97d6-573d7d9b539f/-/scale_crop/155x155/" />
                    </div>
                    <div class="infoCatchWord">
                        Justus
                    </div>
                    <div class="infoExplanationSnippet">
                        (Product Manager)
                    </div>
                </div>
                
                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="https://ucarecdn.com/06b596a8-3787-44d7-9edc-4b9dd493acfd/-/scale_crop/155x155/" />
                    </div>
                    <div class="infoCatchWord">
                        Janine
                    </div>
                    <div class="infoExplanationSnippet">
                        (Designerin)
                    </div>
                </div>
            </div>
            <div class="row infoItemRow">
                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="https://ucarecdn.com/3b330292-fff7-4f7c-8ee8-5d8050a8d1f4/-/scale_crop/155x155/" />
                    </div>
                    <div class="infoCatchWord">
                        Daniel
                    </div>
                    <div class="infoExplanationSnippet">
                        (Developer)
                    </div>
                </div>
                
                <div class="col-xs-4 infoItemColumn">
                    <div class="TeamPic">
                        <img src="https://ucarecdn.com/5174fd26-5db4-43d4-b73c-6f81e872c6dd/-/scale_crop/155x155/" />
                    </div>
                    <div class="infoCatchWord">
                        Marco
                    </div>
                    <div class="infoExplanationSnippet">
                        (Developer)
                    </div>
                </div>
            </div>

        <div class="TeamText">                                          https://memucho.local/Kategorien/memucho-Tutorials/945
            <p class="ShortParagraph">
                memucho ist ein gemeinwohlorientiertes Unternehmen, das freie Bildungsinhalte fördert.<br />
                Unser Team möchte dich beim Lernen unterstützen Dafür konzipieren, gestalten und<br />
                programmieren wir gemeinsam und laden dich ein, es auch auszuprobieren.
            </p>
            <p class="ShortParagraph" id="link-share">
                <a class="btn btn-primary" href="<%=Links.CategoryDetail("memucho-Tutorials",945) %>"><i class="fa fa-lg fa-play-circle">&nbsp;</i>Teile dein Wissen und mache es anderen zugänglich! </a>
            </p>
            <p class="ShortParagraph">
                Wenn du Fragen oder Anregungen hast, schreibe uns eine Email an <a href="mailto:team@memucho.de">team@memucho.de</a>   oder<br />
                rufe uns an: +49 - 1577 - 6825707
            </p>
        </div>
    </div>
    <%= Styles.Render("~/bundles/Team") %>
</asp:Content>
