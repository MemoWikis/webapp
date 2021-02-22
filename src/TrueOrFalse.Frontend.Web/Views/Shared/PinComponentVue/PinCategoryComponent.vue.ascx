    <div>
            <span v-if="stateLoad == 'added'" @click="addToWishknowledge()">
                <i class="fa fa-heart show-tooltip" style="color: #FF001F" title="Aus deinem Wunschwissen entfernen"></i>
            </span>
            <span v-else-if="stateLoad == 'loading'">
                <i class="fa fa-spinner fa-spin" style="color: #FF001F"></i>
            </span>
            <span v-else title="Zu deinem Wunschwissen hinzuzufügen" @click="removeToWishknowledge()">
                <i class="fa fa-heart-o" style="color:#FF001F"></i>
<%--                <span v-if="showAddTxt" class="Text">Hinzufügen</span>--%>
            </span>
    </div>



