﻿<%--<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LoginModel>" %>--%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%= Styles.Render("~/bundles/Registration") %>        

<div id="LoginComponent" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-body">
                
                <button class="close" data-dismiss="modal" style="position: relative; top: -8px; z-index: 1000">×</button>

                <div class="row hide2" id="needs-to-be-logged-in">
                    <div class="col-xs-12 col-you-have-to-be-logged-in">
                                Um diese Funktion zu nutzen, musst du eingeloggt sein. 
                    </div>                
                </div>
            
                <div class="row">
                    <div class="col-xs-10 col-xs-offset-1 xxs-stack" style="padding-bottom: 10px;">
                        <h3>Einloggen mit</h3>                
                    </div>
                </div>
                            
<%--<% using (Html.BeginForm("", "", null, FormMethod.Post, new { id = "LoginForm" })) { %>--%>

    <div class="row">
        
        <div class="form-horizontal col-xs-12" role="form">
            
            <fieldset>
<%--                <% Html.Message(Model.Message); %>--%>

                <div class="form-group omb_login">
                    <div class="omb_socialButtons">
   	                    <div class="col-xs-offset-1 col-xs-5 xxs-stack" style="padding-top: 7px;">
	                           
                            <div class="g-signin2" data-onsuccess="onSignIn"></div>
		                    <a href="#" class="btn btn-block omb_btn-facebook" id="btn-login-with-facebook-modal" style="width: 100%">
			                    <span>Facebook</span>
		                    </a>
	                    </div>
        	            <div class="col-xs-5 xxs-stack" style="padding-top: 7px;">
		                    <a href="#" class="btn btn-block omb_btn-google" id="btn-login-with-google-modal" >
			                    <span>Google+</span>
		                    </a>
	                    </div>	
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-offset-1 col-xs-10 xxs-stack" style="font-size: 12px; padding-top: 7px;">
                        *Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren <a href="<%=Links.TermsAndConditions %>">Nutzungsbedingungen</a>
                        und unserer <a href="<%=Links.Imprint %>">Datenschutzerklärung</a> einverstanden. 
                        <br/><br/>
                        Du musst mind. 16 Jahre alt sein, <a href="/Impressum#under16">hier mehr Infos</a>!
                    </div>
                </div>
                                
                <<div class="row" style="margin-bottom: 10px;">
                    <div class="col-xs-12 col-sm-8 col-sm-offset-2">
                        <div class="register-divider-container">
                            <div class="register-divider">
                                <div class="register-divider-line"></div>
                            </div>
                            <div class="register-divider-label-container">
                                <div class="register-divider-label">
                                    oder
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="row" id="rowLoginMessage" style="display: none">
                    <div class="col-sm-offset-1 col-sm-10 col-xs-12" style="color: red; padding-bottom: 20px;">
                        Du konntest nicht eingeloggt werden. Bitte überprüfe deine E-Mail-Adresse und das Passwort
                    </div>
                </div>

                <div class="form-group">
                 <%--   <%: Html.LabelFor(model => model.EmailAddress, new { @class = "col-xs-offset-1 col-xs-3 xxs-stack control-label" })%>
                    <div class="col-xs-6 xxs-stack">
                        <%= Html.TextBoxFor(m => m.EmailAddress, new { @class="form-control" })%> 
                    </div>
                    <%: Html.ValidationMessageFor(m => m.EmailAddress)%>--%>
                </div>

                <div class="form-group">
                    <label class="col-xs-offset-1 col-xs-3 xxs-stack control-label" for="Password">Passwort</label>
                    <div class="col-xs-6 xxs-stack">
<%--                        <%: Html.PasswordFor(m => m.Password, new { @class="form-control" }) %>--%>
                    </div>
                 </div>
                                 
                <div class="form-group">
                    <div class="col-xs-offset-4 col-xs-8 xxs-stack">
                        <input type="submit" value="Einloggen" class="btn btn-primary memo-button" id="btnModalLogin" /> 

                        <label class="checkbox" style="white-space:nowrap;">
<%--                            <%: Html.CheckBoxFor(model => model.PersistentLogin) %> Eingeloggt bleiben--%>
                        </label>
                        
                    </div>
                </div>
                
                <div class="form-group"> 
                    <div class="col-xs-offset-1 col-xs-10 xxs-stack" style="padding-top: 30px;">
<%--                        <a href="<%= Url.Action("PasswordRecovery", "Welcome") %>">Passwort vergessen?</a><br/>--%>
                        
                        <div style="padding-top: 5px;">
<%--                            <strong>Noch kein Benutzer?</strong>&nbsp; <%: Html.ActionLink("Jetzt registrieren!", Links.RegisterAction, Links.RegisterController) %><br/><br />--%>
                        </div>
                    </div>
                </div>


             </fieldset>
        </div>        
    </div>
            
            <%--<a href="#" data-dismiss="modal" class="btn btn-default" id="btnCloseDateDelete">Schließen</a>--%>
<%--<% } %>--%>

            </div>
        </div>
    </div>
</div>