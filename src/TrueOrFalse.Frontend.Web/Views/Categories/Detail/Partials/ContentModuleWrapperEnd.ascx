<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>
                
 
        </div>

        <div class="ModuleHandle" v-if="hoverState">
            <div class="IconWrapper SortButton" @click="moveUp()">
                <i class="fas fa-chevron-up"></i>
            </div>
            <div class="IconWrapper Handle">
                <i class="fa fa-grip-vertical"></i>
            </div>
            <div class="IconWrapper SortButton" @click="moveDown()">
                <i class="fas fa-chevron-down"></i>
            </div>
        </div>
                                        
        <div class="Button dropdown" v-if="hoverState">
            <a href="#" id="Dropdown" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="Dropdown" style="margin-top:-10px">
                <li><div data-allowed="logged-in" @click.prevent="addModule()"><i class="fa fa-caret-up"></i> Inhalt oben einfügen</div></li>
                <li><div data-allowed="logged-in" @click.prevent="addModule()"><i class="fa fa-caret-down"></i> Inhalt unten einfügen</div></li>
                <li class="delete"><div data-allowed="logged-in" @click.prevent="deleteModule()"><i class="fa fa-trash"></i> Löschen</div></li>
            </ul>
        </div>
    </div>        
</content-module> 
