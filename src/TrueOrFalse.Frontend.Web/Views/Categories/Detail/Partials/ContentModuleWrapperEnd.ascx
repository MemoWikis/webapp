<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>
                
 
        </div>

        <div class="ModuleHandle" v-if="hoverState">
            <div class="ModuleHandleInner">
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
        </div>
        
        <div class="ButtonWrapper" :class="{ moveToCenter : moveOptionsToCenter }">
            <div class="Button dropdown" v-if="hoverState" >
                <div>
                    <a href="#" id="Dropdown" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right module-options" aria-labelledby="Dropdown" style="margin-top:-50px">
                        <li><a data-allowed="logged-in" @click.prevent="addModule('before')"><i class="fa fa-caret-up"></i> Inhalt oben einfügen</a></li>
                        <li><a data-allowed="logged-in" @click.prevent="addModule('after')"><i class="fa fa-caret-down"></i> Inhalt unten einfügen</a></li>
                    </ul>
                </div>
                <div>
                    <a class="btn btn-link btn-sm ButtonEllipsis" data-allowed="logged-in" @click.prevent="deleteModule()"><i class="fa fa-trash"></i></a>
                </div>
            </div>
        </div>


    </div>        
</content-module> 
