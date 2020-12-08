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
                    <a class="btn btn-link btn-sm ButtonEllipsis" data-allowed="logged-in" @click.prevent="deleteModule()"><i class="fa fa-trash"></i></a>
                </div>
            </div>
        </div>


    </div>        
</content-module> 
