    <div class="pin-category-component">
            <div v-if="stateLoad == 'added'" @click="removeFromWishknowledge()">
                <i class="fa fa-heart show-tooltip" style="color: #FF001F" title="Aus deinem Wunschwissen entfernen"></i>
            </div>
            <div v-else-if="stateLoad == 'loading'">
                <i class="fa fa-spinner fa-spin" style="color: #FF001F"></i>
            </div>
            <div v-else title="Zu deinem Wunschwissen hinzuzufügen" @click="addToWishknowledge()">
                <i class="fa fa-heart-o" style="color:#FF001F"></i>
<%--                <span v-if="showAddTxt" class="Text">Hinzufügen</span>--%>
            </div>
    </div>



