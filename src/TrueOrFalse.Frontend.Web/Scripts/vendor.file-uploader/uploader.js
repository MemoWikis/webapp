/**
 * Class that creates upload widget with drag-and-drop and file list
 * @inherits qq.FineUploaderBasic
 */
qq.FineUploader = function(o){
    // call parent constructor
    qq.FineUploaderBasic.apply(this, arguments);

    // additional options
    qq.extend(this._options, {
        element: null,
        listElement: null,
        dragAndDrop: {
            extraDropzones: [],
            hideDropzones: true,
            disableDefaultDropzone: false
        },
        text: {
            uploadButton: 'Upload a file',
            cancelButton: 'Cancel',
            retryButton: 'Retry',
            failUpload: 'Upload failed',
            dragZone: 'Drop files here to upload',
            dropProcessing: 'Processing dropped files...',
            formatProgress: "{percent}% of {total_size}",
            waitingForResponse: "Processing..."
        },
        template: '<div class="qq-uploader">' +
            ((!this._options.dragAndDrop || !this._options.dragAndDrop.disableDefaultDropzone) ? '<div class="qq-upload-drop-area"><span>{dragZoneText}</span></div>' : '') +
            (!this._options.button ? '<div class="qq-upload-button"><div>{uploadButtonText}</div></div>' : '') +
            '<span class="qq-drop-processing"><span>{dropProcessingText}</span><span class="qq-drop-processing-spinner"></span></span>' +
            (!this._options.listElement ? '<ul class="qq-upload-list"></ul>' : '') +
            '</div>',

        // template for one item in file list
        fileTemplate: '<li>' +
            '<div class="qq-progress-bar"></div>' +
            '<span class="qq-upload-spinner"></span>' +
            '<span class="qq-upload-finished"></span>' +
            '<span class="qq-upload-file"></span>' +
            '<span class="qq-upload-size"></span>' +
            '<a class="qq-upload-cancel" href="#">{cancelButtonText}</a>' +
            '<a class="qq-upload-retry" href="#">{retryButtonText}</a>' +
            '<span class="qq-upload-status-text">{statusText}</span>' +
            '</li>',
        classes: {
            button: 'qq-upload-button',
            drop: 'qq-upload-drop-area',
            dropActive: 'qq-upload-drop-area-active',
            dropDisabled: 'qq-upload-drop-area-disabled',
            list: 'qq-upload-list',
            progressBar: 'qq-progress-bar',
            file: 'qq-upload-file',
            spinner: 'qq-upload-spinner',
            finished: 'qq-upload-finished',
            retrying: 'qq-upload-retrying',
            retryable: 'qq-upload-retryable',
            size: 'qq-upload-size',
            cancel: 'qq-upload-cancel',
            retry: 'qq-upload-retry',
            statusText: 'qq-upload-status-text',

            success: 'qq-upload-success',
            fail: 'qq-upload-fail',

            successIcon: null,
            failIcon: null,

            dropProcessing: 'qq-drop-processing',
            dropProcessingSpinner: 'qq-drop-processing-spinner'
        },
        failedUploadTextDisplay: {
            mode: 'default', //default, custom, or none
            maxChars: 50,
            responseProperty: 'error',
            enableTooltip: true
        },
        messages: {
            tooManyFilesError: "You may only drop one file"
        },
        retry: {
            showAutoRetryNote: true,
            autoRetryNote: "Retrying {retryNum}/{maxAuto}...",
            showButton: false
        },
        showMessage: function(message){
            setTimeout(function() {
                alert(message);
            }, 0);
        }
    }, true);

    // overwrite options with user supplied
    qq.extend(this._options, o, true);
    this._wrapCallbacks();

    // overwrite the upload button text if any
    // same for the Cancel button and Fail message text
    this._options.template     = this._options.template.replace(/\{dragZoneText\}/g, this._options.text.dragZone);
    this._options.template     = this._options.template.replace(/\{uploadButtonText\}/g, this._options.text.uploadButton);
    this._options.template     = this._options.template.replace(/\{dropProcessingText\}/g, this._options.text.dropProcessing);
    this._options.fileTemplate = this._options.fileTemplate.replace(/\{cancelButtonText\}/g, this._options.text.cancelButton);
    this._options.fileTemplate = this._options.fileTemplate.replace(/\{retryButtonText\}/g, this._options.text.retryButton);
    this._options.fileTemplate = this._options.fileTemplate.replace(/\{statusText\}/g, "");

    this._element = this._options.element;
    this._element.innerHTML = this._options.template;
    this._listElement = this._options.listElement || this._find(this._element, 'list');

    this._classes = this._options.classes;

    if (!this._button) {
        this._button = this._createUploadButton(this._find(this._element, 'button'));
    }

    this._bindCancelAndRetryEvents();

    this._dnd = this._setupDragAndDrop();
};

// inherit from Basic Uploader
qq.extend(qq.FineUploader.prototype, qq.FineUploaderBasic.prototype);

qq.extend(qq.FineUploader.prototype, {
    clearStoredFiles: function() {
        qq.FineUploaderBasic.prototype.clearStoredFiles.apply(this, arguments);
        this._listElement.innerHTML = "";
    },
    addExtraDropzone: function(element){
        this._dnd.setupExtraDropzone(element);
    },
    removeExtraDropzone: function(element){
        return this._dnd.removeExtraDropzone(element);
    },
    getItemByFileId: function(id){
        var item = this._listElement.firstChild;

        // there can't be txt nodes in dynamically created list
        // and we can  use nextSibling
        while (item){
            if (item.qqFileId == id) return item;
            item = item.nextSibling;
        }
    },
    cancel: function(fileId) {
        qq.FineUploaderBasic.prototype.cancel.apply(this, arguments);
        var item = this.getItemByFileId(fileId);
        qq(item).remove();
    },
    reset: function() {
        qq.FineUploaderBasic.prototype.reset.apply(this, arguments);
        this._element.innerHTML = this._options.template;
        this._listElement = this._options.listElement || this._find(this._element, 'list');
        if (!this._options.button) {
            this._button = this._createUploadButton(this._find(this._element, 'button'));
        }
        this._bindCancelAndRetryEvents();
        this._dnd.dispose();
        this._dnd = this._setupDragAndDrop();
    },
    _setupDragAndDrop: function() {
        var self = this,
            dropProcessingEl = this._find(this._element, 'dropProcessing'),
            dnd, preventSelectFiles, defaultDropAreaEl;

        preventSelectFiles = function(event) {
            event.preventDefault();
        };

        if (!this._options.dragAndDrop.disableDefaultDropzone) {
            defaultDropAreaEl = this._find(this._options.element, 'drop');
        }

        dnd = new qq.DragAndDrop({
            dropArea: defaultDropAreaEl,
            extraDropzones: this._options.dragAndDrop.extraDropzones,
            hideDropzones: this._options.dragAndDrop.hideDropzones,
            multiple: this._options.multiple,
            classes: {
                dropActive: this._options.classes.dropActive
            },
            callbacks: {
                dropProcessing: function(isProcessing, files) {
                    var input = self._button.getInput();

                    if (isProcessing) {
                        qq(dropProcessingEl).css({display: 'block'});
                        qq(input).attach('click', preventSelectFiles);
                    }
                    else {
                        qq(dropProcessingEl).hide();
                        qq(input).detach('click', preventSelectFiles);
                    }

                    if (files) {
                        self.addFiles(files);
                    }
                },
                error: function(code, filename) {
                    self._error(code, filename);
                },
                log: function(message, level) {
                    self.log(message, level);
                }
            }
        });

        dnd.setup();

        return dnd;
    },
    _leaving_document_out: function(e){
        return ((qq.chrome() || (qq.safari() && qq.windows())) && e.clientX == 0 && e.clientY == 0) // null coords for Chrome and Safari Windows
            || (qq.firefox() && !e.relatedTarget); // null e.relatedTarget for Firefox
    },
    _storeFileForLater: function(id) {
        qq.FineUploaderBasic.prototype._storeFileForLater.apply(this, arguments);
        var item = this.getItemByFileId(id);
        qq(this._find(item, 'spinner')).hide();
    },
    /**
     * Gets one of the elements listed in this._options.classes
     **/
    _find: function(parent, type){
        var element = qq(parent).getByClass(this._options.classes[type])[0];
        if (!element){
            throw new Error('element not found ' + type);
        }

        return element;
    },
    _onSubmit: function(id, fileName){
        qq.FineUploaderBasic.prototype._onSubmit.apply(this, arguments);
        this._addToList(id, fileName);
    },
    // Update the progress bar & percentage as the file is uploaded
    _onProgress: function(id, fileName, loaded, total){
        qq.FineUploaderBasic.prototype._onProgress.apply(this, arguments);

        var item, progressBar, text, percent, cancelLink, size;

        item = this.getItemByFileId(id);
        progressBar = this._find(item, 'progressBar');
        percent = Math.round(loaded / total * 100);

        if (loaded === total) {
            cancelLink = this._find(item, 'cancel');
            qq(cancelLink).hide();

            qq(progressBar).hide();
            qq(this._find(item, 'statusText')).setText(this._options.text.waitingForResponse);

            // If last byte was sent, just display final size
            text = this._formatSize(total);
        }
        else {
            // If still uploading, display percentage
            text = this._formatProgress(loaded, total);

            qq(progressBar).css({display: 'block'});
        }

        // Update progress bar element
        qq(progressBar).css({width: percent + '%'});

        size = this._find(item, 'size');
        qq(size).css({display: 'inline'});
        qq(size).setText(text);
    },
    _onComplete: function(id, fileName, result, xhr){
        qq.FineUploaderBasic.prototype._onComplete.apply(this, arguments);

        var item = this.getItemByFileId(id);

        qq(this._find(item, 'statusText')).clearText();

        qq(item).removeClass(this._classes.retrying);
        qq(this._find(item, 'progressBar')).hide();

        if (!this._options.disableCancelForFormUploads || qq.isXhrUploadSupported()) {
            qq(this._find(item, 'cancel')).hide();
        }
        qq(this._find(item, 'spinner')).hide();

        if (result.success){
            qq(item).addClass(this._classes.success);
            if (this._classes.successIcon) {
                this._find(item, 'finished').style.display = "inline-block";
                qq(item).addClass(this._classes.successIcon);
            }
        } else {
            qq(item).addClass(this._classes.fail);
            if (this._classes.failIcon) {
                this._find(item, 'finished').style.display = "inline-block";
                qq(item).addClass(this._classes.failIcon);
            }
            if (this._options.retry.showButton && !this._preventRetries[id]) {
                qq(item).addClass(this._classes.retryable);
            }
            this._controlFailureTextDisplay(item, result);
        }
    },
    _onUpload: function(id, fileName){
        qq.FineUploaderBasic.prototype._onUpload.apply(this, arguments);

        var item = this.getItemByFileId(id);
        this._showSpinner(item);
    },
    _onBeforeAutoRetry: function(id) {
        var item, progressBar, cancelLink, failTextEl, retryNumForDisplay, maxAuto, retryNote;

        qq.FineUploaderBasic.prototype._onBeforeAutoRetry.apply(this, arguments);

        item = this.getItemByFileId(id);
        progressBar = this._find(item, 'progressBar');

        this._showCancelLink(item);
        progressBar.style.width = 0;
        qq(progressBar).hide();

        if (this._options.retry.showAutoRetryNote) {
            failTextEl = this._find(item, 'statusText');
            retryNumForDisplay = this._autoRetries[id] + 1;
            maxAuto = this._options.retry.maxAutoAttempts;

            retryNote = this._options.retry.autoRetryNote.replace(/\{retryNum\}/g, retryNumForDisplay);
            retryNote = retryNote.replace(/\{maxAuto\}/g, maxAuto);

            qq(failTextEl).setText(retryNote);
            if (retryNumForDisplay === 1) {
                qq(item).addClass(this._classes.retrying);
            }
        }
    },
    //return false if we should not attempt the requested retry
    _onBeforeManualRetry: function(id) {
        if (qq.FineUploaderBasic.prototype._onBeforeManualRetry.apply(this, arguments)) {
            var item = this.getItemByFileId(id);
            this._find(item, 'progressBar').style.width = 0;
            qq(item).removeClass(this._classes.fail);
            qq(this._find(item, 'statusText')).clearText();
            this._showSpinner(item);
            this._showCancelLink(item);
            return true;
        }
        return false;
    },
    _addToList: function(id, fileName){
        var item = qq.toElement(this._options.fileTemplate);
        if (this._options.disableCancelForFormUploads && !qq.isXhrUploadSupported()) {
            var cancelLink = this._find(item, 'cancel');
            qq(cancelLink).remove();
        }

        item.qqFileId = id;

        var fileElement = this._find(item, 'file');
        qq(fileElement).setText(this._options.formatFileName(fileName));
        qq(this._find(item, 'size')).hide();
        if (!this._options.multiple) this._clearList();
        this._listElement.appendChild(item);
    },
    _clearList: function(){
        this._listElement.innerHTML = '';
        this.clearStoredFiles();
    },
    /**
     * delegate click event for cancel & retry links
     **/
    _bindCancelAndRetryEvents: function(){
        var self = this,
            list = this._listElement;

        this._disposeSupport.attach(list, 'click', function(e){
            e = e || window.event;
            var target = e.target || e.srcElement;

            if (qq(target).hasClass(self._classes.cancel) || qq(target).hasClass(self._classes.retry)){
                qq.preventDefault(e);

                var item = target.parentNode;
                while(item.qqFileId == undefined) {
                    item = target = target.parentNode;
                }

                if (qq(target).hasClass(self._classes.cancel)) {
                    self.cancel(item.qqFileId);
                }
                else {
                    qq(item).removeClass(self._classes.retryable);
                    self.retry(item.qqFileId);
                }
            }
        });
    },
    _formatProgress: function (uploadedSize, totalSize) {
        var message = this._options.text.formatProgress;
        function r(name, replacement) { message = message.replace(name, replacement); }

        r('{percent}', Math.round(uploadedSize / totalSize * 100));
        r('{total_size}', this._formatSize(totalSize));
        return message;
    },
    _controlFailureTextDisplay: function(item, response) {
        var mode, maxChars, responseProperty, failureReason, shortFailureReason;

        mode = this._options.failedUploadTextDisplay.mode;
        maxChars = this._options.failedUploadTextDisplay.maxChars;
        responseProperty = this._options.failedUploadTextDisplay.responseProperty;

        if (mode === 'custom') {
            failureReason = response[responseProperty];
            if (failureReason) {
                if (failureReason.length > maxChars) {
                    shortFailureReason = failureReason.substring(0, maxChars) + '...';
                }
            }
            else {
                failureReason = this._options.text.failUpload;
                this.log("'" + responseProperty + "' is not a valid property on the server response.", 'warn');
            }

            qq(this._find(item, 'statusText')).setText(shortFailureReason || failureReason);

            if (this._options.failedUploadTextDisplay.enableTooltip) {
                this._showTooltip(item, failureReason);
            }
        }
        else if (mode === 'default') {
            qq(this._find(item, 'statusText')).setText(this._options.text.failUpload);
        }
        else if (mode !== 'none') {
            this.log("failedUploadTextDisplay.mode value of '" + mode + "' is not valid", 'warn');
        }
    },
    //TODO turn this into a real tooltip, with click trigger (so it is usable on mobile devices).  See case #355 for details.
    _showTooltip: function(item, text) {
        item.title = text;
    },
    _showSpinner: function(item) {
        var spinnerEl = this._find(item, 'spinner');
        spinnerEl.style.display = "inline-block";
    },
    _showCancelLink: function(item) {
        if (!this._options.disableCancelForFormUploads || qq.isXhrUploadSupported()) {
            var cancelLink = this._find(item, 'cancel');
            cancelLink.style.display = 'inline';
        }
    },
    _error: function(code, fileName){
        var message = qq.FineUploaderBasic.prototype._error.apply(this, arguments);
        this._options.showMessage(message);
    }
});
