<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>
                
                <div class="Button Handle" v-if="hoverState">
                    <i class="fa fa-bars"></i>
                </div>
                                
                <div class="Button dropdown" v-if="hoverState">
                    <a href="#" id="Dropdown" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="Dropdown" style="margin-top:-10px">
                        <li><a href="" data-allowed="logged-in" role="button" data-toggle="modal" :data-target="modalType" :data-component-id="id" :data-markdown="origMarkdown" @click="isListening = true"><i class="fa fa-copy"></i> Modul konfigurieren</a></li>
                        <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-up"></i> Inhalt oben einfügen</a></li>
                        <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-down"></i> Inhalt unten einfügen</a></li>
                        <li class="delete"><a href="" data-allowed="logged-in" @click.prevent="deleteModule()"><i class="fa fa-trash"></i> Löschen</a></li>
                    </ul>
                </div>


            </div>    
        </div>     
    </li>        
</content-module> 
