<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LoginModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%= Styles.Render("~/bundles/Registration") %>        

<div id="modalLogin" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
        <div class="modal-body">
            
            <div class="row">
                <div class="col-sm-offset-1" style="padding-left: 5px; padding-bottom: 10px;">
                    <h3>Einloggen mit</h3>                
                </div>
            </div>
                
<% using (Html.BeginForm("", "", null, FormMethod.Post, new { id = "LoginForm" })) { %>

    <div class="row">
        
        <div class="form-horizontal col-md-12" role="form">
            
            <fieldset>
                <% Html.Message(Model.Message); %>
                
                <div class="form-group omb_login">
                    <div class="row omb_socialButtons">
   	                    <div class="col-sm-offset-1 col-xs-12 col-sm-5" style="padding-top: 7px;">
	                           
                            <div class="g-signin2" data-onsuccess="onSignIn"></div>
		                    <a href="#" class="btn btn-block omb_btn-facebook" id="btn-login-with-facebook-modal" style="width: 100%">
			                    <span>Facebook</span>
		                    </a>
	                    </div>
        	            <div class="col-xs-12 col-sm-5" style="padding-top: 7px;">
		                    <a href="#" class="btn btn-block omb_btn-google" id="btn-login-with-google-modal" >
			                    <span>Google+</span>
		                    </a>
	                    </div>	
                    </div>
                    
                    <div class="form-group">
                        <div class="col-sm-offset-1 col-sm-10" style="font-size: 12px; padding-top: 7px;">
                            *Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren <a href="<%=Links.TermsAndConditions %>">Nutzungsbedingungen</a>
                            und unserer <a href="<%=Links.Imprint %>">Datenschutzrichtlinie</a> einverstanden. 
                        </div>
                    </div>
                </div>
                                
                <div class="row" style="margin-top: 20px; margin-bottom: 5px;">
                    <div class="col-sm-offset-1 col-sm-4 col-xs-5" style="border-bottom: 1px solid silver"></div>
                    <div class="col-sm-2 col-xs-2" style="text-align: center"><span style="position: relative; top: -9px;">oder</span></div>
                    <div class="col-sm-4 col-xs-5" style="border-bottom: 1px solid silver"></div>
                </div>
                
                <div class="row" id="rowLoginMessage" style="display: none">
                    <div class="col-sm-offset-1 col-sm-10" style="color: red; padding-bottom: 20px;">
                        Du konntest nicht eingeloggt werden. Bitte überprüfe deine E-Mail-Adresse und das Passwort
                    </div>
                </div>

                <div class="form-group">
                    <%: Html.LabelFor(model => model.EmailAddress, new { @class = "col-sm-offset-1 col-sm-3 control-label" })%>
                    <div class="col-sm-6">
                        <%= Html.TextBoxFor(m => m.EmailAddress, new { @class="form-control" })%> 
                    </div>
                    <%: Html.ValidationMessageFor(m => m.EmailAddress)%>
                </div>

                <div class="form-group">
                    <label class="col-sm-offset-1 col-sm-3 control-label" for="Password">Passwort</label>
                    <div class="col-sm-6">
                        <%: Html.PasswordFor(m => m.Password, new { @class="form-control" }) %>
                    </div>
                 </div>
                                 
                <div class="form-group">
                    <div class="col-sm-offset-4 col-sm-8">
                        <input type="submit" value="Einloggen" class="btn btn-primary" id="btnModalLogin" /> 

                        <label class="checkbox" style="white-space:nowrap;">
                            <%: Html.CheckBoxFor(model => model.PersistentLogin) %> Eingeloggt bleiben
                        </label>
                        
                    </div>
                </div>
                
                <div class="form-group"> 
                    <div class="col-sm-offset-1 col-sm-10" style="padding-top: 30px;">
                        <a href="<%= Url.Action("PasswordRecovery", "Welcome") %>">Passwort vergessen?</a><br/>
                        
                        <div style="padding-top: 5px;">
                            Noch kein Benutzer?&nbsp; <%: Html.ActionLink("Jetzt registrieren!", Links.RegisterAction, Links.RegisterController) %><br/><br />
                        </div>
                    </div>
                </div>


             </fieldset>
        </div>        
    </div>
            
            <%--<a href="#" data-dismiss="modal" class="btn btn-default" id="btnCloseDateDelete">Schließen</a>--%>
<% } %>

            </div>
        </div>
    </div>
</div>