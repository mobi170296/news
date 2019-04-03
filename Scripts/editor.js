function $qr(){
    this.selector = '';
    this.length = 0;
    $qr.prototype.__init = function(s){
        this.selector = s;
        var r=document.querySelectorAll(s);
        this.length=r.length;
        for(var i=0;i<this.length;i++){
            this[i]=r[i];
        }
        return this;
    }
    $qr.prototype.css = function(n,v){
        var a=n.split('-');
        for(var i=1;i<a.length;i++){
            a[i]=a[i][0].toUpperCase() + a[i].substr(1);
        }
        n=a.join('');
        for (var i = 0; i < this.length; i++){
            if (this[i].style!==undefined) {
                this[i].style[n] = v;
            }
        }
        return this;
    }
    $qr.prototype.joinAndCapitalize = function (n) {
        var a = n.split('-');
        for (var i = 1; i < a.length; i++) {
            a[i] = a[i][0].toUpperCase() + a[i].substr(1);
        }
        return a.join('');
    }
    $qr.prototype.text = function(t=null){
        if(t===null){
            if(this.length){
                return this[0].innerText;
            }else{
                return null;
            }
        }else{
            if(this.length){
                for (var i = 0; i < this.length; i++){
                    this[i].innerText = t;
                }
            }
        }
    }
    $qr.prototype.html = function(t=null){
        if(t===null){
            if(this.length){
                return this[0].innerHTML;
            }else{
                return null;
            }
        }else{
            if(this.length){
                for(var i=0;i<this.length;i++){
                    this[i].innerHTML = t;
                }
            }
        }
    }
    $qr.prototype.val = function(t=null){
        if(t===null){
            if(this.length){
                return this[0].value===undefined?'':this[0].value;
            }else{
                return undefined;
            }
        }else{
            if(this.length){
                for(var i=0;i<this.length;i++){
                    if(this[i].value!==undefined){
                        this[i].value = t;
                    }
                }
            }
        }
    }
    $qr.prototype.oncontextmenu = function (c) {
        for (var i = 0; i < this.length; i++) {
            if (this[i].oncontextmenu !== undefined) {
                this[i].oncontextmenu = c;
            }
        }
        return this;
    }
    $qr.prototype.on = function(e,c){
        for(var i=0;i<this.length;i++){
            this[i].addEventListener(e,c);
        }
        return this;
    }
    $qr.prototype.off = function(e,c){
        for(var i=0;i<this.length;i++){
            this[i].removeEventListener(e,c);
        }
        return this;
    }
    $qr.prototype.haveClass = function (c) {
        if (this.length && typeof this[0].classList != 'undefined') {
            return this[0].classList.contains(c);
        }
        return false;
    }
    $qr.prototype.addClass = function(c){
        for(var i=0;i<this.length;i++){
            this[i].classList.add(c);
        }
        return this;
    }
    $qr.prototype.removeClass = function(c){
        for(var i=0;i<this.length;i++){
            this[i].classList.remove(c);
        }
        return this;
    }
    $qr.prototype.data = function (f, v = undefined) {
        if (v === undefined) {
            if (this.length) {
                return this[0].dataset[this.joinAndCapitalize(f)];
            }
        } else {
            for (var i = 0; i < this.length; i++) {
                this[i].dataset[this.joinAndCapitalize(f)] = v;
            }
            return this;
        }
    }
    $qr.prototype.cssData = function (p, d) {
        for (var i = 0; i < this.length; i++) {
            $(this[i]).css(p, $(this[i]).data(d));
        }
        return this;
    }
    $qr.prototype.attr = function (k, v = null) {
        if (arguments.length === 1) {
            if (this.length && typeof this[0].getAttribute === 'function') {
                return this[0].getAttribute(k);
            } else {
                return null;
            }
        } else {
            for (var i = 0; i < this.length; i++) {
                if (typeof this[i].setAttribute === 'function') {
                    this[i].setAttribute(k, v);
                }
            }
            return this;
        }
    }
	$qr.prototype.next = function(){
		if(this.length){
			return $(this[0].nextElementSibling);
		}
	}
	$qr.prototype.previous = function(){
		if(this.length){
			return $(this[0].previousElementSibling);
		}
    }
    $qr.prototype.parent = function () {
        if (this.length) {
            return $(this[0].parentElement);
        }
    }
    $qr.prototype.child = function () {
        if (this.length && typeof this[0].children != 'undefined') {
            return $(this[0].children);
        } else {
            var r = new $qr();
            r.length = 0;
            return r;
        }
    }
	$qr.prototype.remove = function(){
		for(var i=0;i<this.length;i++){
			this[i].remove();
		}
    }
    $qr.prototype.append = function (o) {
        if (this.length && typeof this[0].appendChild === "function") {
            this[0].appendChild(o);
        }
    }
    $qr.prototype.addBegin = function (o) {
        if (this.length && typeof this[0].insertBefore === "function") {
            this[0].insertBefore(o, this[0].children[0]);
        }
        return this;
    }
    $qr.prototype.addEnd = function (o) {
        if (this.length && typeof this[0].appendChild === "function") {
            this[0].appendChild(o);
        }
        return this;
    }
    $qr.prototype.addNext = function (e) {
        if (this.length && typeof this[0].insertAdjacentElement === "function") {
            this[0].insertAdjacentElement('afterend', e);
        }
        return this;
    }
    $qr.prototype.addPrevious = function (e) {
        if (this.length && typeof this[0].insertAdjacentElement === "function") {
            this[0].insertAdjacentElement('beforebegin', e);
        }
        return this;
    }
    $qr.prototype.$ = function (s) {
        if (this.length) {
            if (typeof this[0].querySelectorAll === 'function') {
                var es = this[0].querySelectorAll(s);
                var r = new $qr();
                r.length = es.length;
                for (var i = 0; i < es.length; i++) {
                    r[i] = es[i];
                }
                return r;
            }
        }
        var r = new $qr();
        r.length = 0;
        return r;
    }
}


function $(s) {
    var r = new $qr();
    switch(typeof(s)){
        case 'string':
            return r.__init(s);
        case 'object':
            if(s instanceof $qr){
                return s;
            }
            if(s instanceof Element || s instanceof Node){
                r.length = 1;
                r[0] = s;
                return r;
            }
            if (s instanceof Array || s instanceof HTMLCollection) {
                r.length = s.length;
                for(var i=0;i<s.length;i++){
                    r[i]=s[i];
                }
                return r;
            }
    }
    if (s == null) {
        r.length = 0;
        return r;
    }
    r.length = 1;
    r[0] = s;
    return r;
}


$.create = function (n, p = {}){
    var e = document.createElement(n);
    for (var x in p) {
        e.style[x] = p[x];
    }
    return e;
}

/*
 * Rich Editor section
 * 
*/

function RichEditorSelect(t='') {
    this.getEditor = function () {
        return this.window;
    }

    this.setSelectedText = function (s) {
        $(this.selected).html(s);
    }

    this.select = function (n) {
        n = n.toLowerCase();
        $(this.selectItems).removeClass('active');
        for (var i = 0; i < $(this.selectItems).child().length; i++) {
            if ($($(this.selectItems).child()[i]).html().toLowerCase() === n) {
                $(this.selectItems).child().removeClass('active');
                $($(this.selectItems).child()[i]).addClass('active');
                this.setSelectedText($($(this.selectItems).child()[i]).html());
                return;
            }
        }
    }

    this.getContainer = function () {
        return this.container;
    }

    this.addItem = function (t, f) {
        var item = $.create('div');
        $(item).html(t);
        $(item).addClass('custom-select-item');
        $(item).on('click', function (e) {
            $(this).parent().child().removeClass('active');
            $(this).addClass('active');
            $($(this).parent().parent().child()[0]).html($(this).html());
            $(this).parent().addClass('u-hidden');
        });
        item.onclick = f;
        $(this.selectItems).addEnd(item);
        return item;
    }

    this.toggleSelectItems = function () {
        if ($(this.selectItems).haveClass('u-hidden')) {
            $(this.selectItems).removeClass('u-hidden');
        } else {
            $(this.selectItems).addClass('u-hidden');
        }
    }

    this.selected = $.create('div');
    $(this.selected).html(t);
    $(this.selected).addClass('custom-select-selected');
    this.selected.__container = this;
    $(this.selected).on('click', function (e) {
        this.__container.toggleSelectItems();
    });

    this.selectItems = $.create('div');
    $(this.selectItems).data('role', 'popup').data('role-type', 'static');
    $(this.selectItems).addClass('custom-select-items').addClass('u-hidden');


    this.container = $.create('div');
    $(this.container).addClass('richeditor-custom-select');
    $(this.container).addEnd(this.selected).addEnd(this.selectItems);
    $(this.container).on('click', function (e) {
        e.stopPropagation();
    });
}

function RichEditorColorPicker() {
    this.getContainer = function(){
        return this.container;
    }
    this.addColor = function (c) {
        var cn = $.create('span');
        $(cn).addClass('color-node').data('color', c);
        $(cn).cssData('background-color', 'color');
        $(this.colorTable).addEnd(cn);
        cn.richEditorColorPicker = this;
        $(cn).on('click', function (e) {
            this.richEditorColorPicker.hideColorTable();
            this.richEditorColorPicker.setColor($(this).data('color'));
        });
        return this;
    }

    this.showColorTable = function () {
        this.colorTable.classList.remove('u-hidden');
    }

    this.hideColorTable = function () {
        $(this.colorTable).addClass('u-hidden');
    }

    this.setColor = function (c) {
        $(this.currentColor).data('color', c).cssData('background-color', 'color');
        if (this.onchangecolor != null) {
            this.onchangecolor(c);
        }
    }

    this.onchangecolor = null;

    this.container = $.create('div');
    $(this.container).on('click', function (e) {
        e.stopPropagation();
    });
    $(this.container).addClass('color-control');
    this.container.richEditorColorPicker = this;

    this.currentColor = $.create('span');
    $(this.currentColor).addClass('color-node').addClass('current-color');
    this.currentColor.richEditorColorPicker = this;
    $(this.currentColor).on('click', function (e) {
        if (this.richEditorColorPicker.onchangecolor != null) {
            this.richEditorColorPicker.onchangecolor($(this).data('color'));
        }
    });

    this.toggleColorTable = function () {
        if ($(this.colorTable).haveClass('u-hidden')) {
            $(this.colorTable).removeClass('u-hidden');
        } else {
            $(this.colorTable).addClass('u-hidden');
        }
    }

    this.colorDropdown = $.create('span');
    $(this.colorDropdown).addClass('color-control-dropdown');
    this.colorDropdown.__container = this;
    $(this.colorDropdown).on('click', function (e) {
        this.__container.toggleColorTable();
    });
    this.colorDropdown.richEditorColorPicker = this;

    this.colorTable = $.create('div');
    $(this.colorTable).data('role', 'popup').data('role-type', 'static');
    $(this.colorTable).addClass('color-table').addClass('u-hidden');
    this.colorTable.richEditorColorPicker = this;
    $(this.colorTable).on('click', function (e) {
        e.stopPropagation();
    });

    $(this.container).addEnd(this.currentColor).addEnd(this.colorDropdown).addEnd(this.colorTable);

    //close picker when click out
    $(document).on('click', function (e) {
        $('.color-control .color-table').addClass('u-hidden');
    });
}

function ImagePropertyPopup(img) {
    this.image = img;
    this.getContainer = function () {
        return this.container;
    }
    this.container = $.create('div');
    $(this.container).data('role', 'popup').data('role-type', 'dynamic');
    $(this.container).addClass('img-pro-context');
    $(this.container).html('<div class="content"><table><tr><td><input type="text" name="width" size="4"/></td><td>px</td></tr><tr><td><input type="text" name="height" size="4"/></td><td>px</td></tr></table></div><div class="footer"><button type="button" data-role="close">Đóng</button><button type="button" data-role="apply">Áp dụng</button>');


    this.widthText = $(this.container).$('input[name="width"]')[0];
    this.heightText = $(this.container).$('input[name="height"]')[0];
    this.applyButton = $(this.container).$('button[data-role="apply"]')[0];
    this.applyButton.__container = this;
    $(this.applyButton).on('click', function (e) {
        this.__container.setImageSize();
        this.__container.close();
    });
    this.closeButton = $(this.container).$('button[data-role="close"]')[0];
    this.closeButton.__container = this;
    $(this.closeButton).on('click', function (e) {
        this.__container.close();
    });
    this.getWidth = function () {
        return this.widthText.value;
    }
    this.getHeight = function () {
        return this.heightText.value;
    }
    this.setImageSize = function () {
        this.image.width = this.getWidth();
        this.image.height = this.getHeight();
    }
    this.move = function (x, y) {
        $(this.container).css('left', x + 'px');
        $(this.container).css('top', y + 'px');
    }
    this.close = function () {
        this.container.remove();
    }
    $(this.container).on('contextmenu', function (e) {
        e.stopPropagation();
    });
    $(this.container).on('click', function (e) {
        e.stopPropagation();
    });
    this.show = function (x, y) {
        this.move(x, y);
        $(document.body).addEnd(this.container);
    }
}

function RichEditor(textarea) {
    "use strict"
    if (textarea.nodeName !== "TEXTAREA") {
        return null;
    }
    var ne = textarea.nextElementSibling;
    var pe = textarea.parentElement;
    $(textarea).css('display', 'none');

    this.makeTableResizable = function () {

    }

    this.container = $.create('div');
    $(this.container).addClass('richeditor');

    this.getContainer = function () {
        return this.container;
    }

    this.toolbox = $.create('div');
    $(this.toolbox).addClass('toolbox');
    $(this.toolbox).on('mousedown', function (e) {
        e.preventDefault();
    });

    this.editor = $.create('div');
    //$(this.editor).attr('contenteditable', true);
    //$(this.editor).attr('tabindex', 0);
    //references objects
    this.editor.textarea = textarea;
    this.editor.richeditor = this;
    //set innerHTML => value
    $(this.editor).html($(textarea).val());
    $(this.editor).addClass('editor');
    $(this.editor).on('input', function (e) {
        $('div.richeditor div.editor *').attr('tabindex', 0);
        $(this.textarea).text($(this).html());
    });
    $(this.editor).on('click', function (e) {
        this.contentEditable = true;
        this.tabindex = 0;
        this.richeditor.updateToolBox();
    });

    $(this.editor).on('keyup', function (e) {
        this.richeditor.updateToolBox();
    });

    this.supportButtons = ['backColor', 'bold', 'copy', 'createLink', 'cut', 'decreaseFontSize', 'delete', 'fontName', 'fontSize', 'foreColor', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'increaseFontSize', 'indent', 'insertBrOnReturn', 'insertHorizontalRule', 'insertHTML', 'insertOrderedList', 'insertUnorderedList', 'insertPagagraph', 'insertText', 'italic', 'justifyCenter', 'justifyFull', 'justifyLeft', 'justifyRight', 'outdent', 'redo', 'removeFormat', 'selectAll', 'strikeThrough', 'subscript', 'superscript', 'underline', 'undo', 'unlink', 'styleWithCSS', 'forwardDelete', 'remove', 'insertImage'];

    for (var i = 0; i < this.supportButtons.length; i++) {
        var bn = this.supportButtons[i] + 'Button';
        this[bn] = $.create('span');
        $(this[bn]).addClass('button');
        this[bn].__richEditor = this;
        $(this[bn]).addClass(bn);
    }

    this.updateToolBox = function () {
        if (document.queryCommandState('bold')) {
            $(this.boldButton).addClass('active');
        } else {
            $(this.boldButton).removeClass('active');
        }

        if (document.queryCommandState('italic')) {
            $(this.italicButton).addClass('active');
        } else {
            $(this.italicButton).removeClass('active');
        }

        if (document.queryCommandState('underline')) {
            $(this.underlineButton).addClass('active');
        } else {
            $(this.underlineButton).removeClass('active');
        }

        if (document.queryCommandState('strikeThrough')) {
            $(this.strikeThroughButton).addClass('active');
        } else {
            $(this.strikeThroughButton).removeClass('active');
        }

        if (document.queryCommandState('justifyLeft')) {
            $(this.justifyLeftButton).addClass('active');
        } else {
            $(this.justifyLeftButton).removeClass('active');
        }

        if (document.queryCommandState('justifyCenter')) {
            $(this.justifyCenterButton).addClass('active');
        } else {
            $(this.justifyCenterButton).removeClass('active');
        }

        if (document.queryCommandState('justifyRight')) {
            $(this.justifyRightButton).addClass('active');
        } else {
            $(this.justifyRightButton).removeClass('active');
        }

        if (document.queryCommandState('justifyFull')) {
            $(this.justifyFullButton).addClass('active');
        } else {
            $(this.justifyFullButton).removeClass('active');
        }

        if (document.queryCommandState('insertUnorderedList')) {
            $(this.insertUnorderedListButton).addClass('active');
        } else {
            $(this.insertUnorderedListButton).removeClass('active');
        }

        if (document.queryCommandState('insertOrderedList')) {
            $(this.insertOrderedListButton).addClass('active');
        } else {
            $(this.insertOrderedListButton).removeClass('active');
        }

        if (document.queryCommandState('subscript')) {
            $(this.subscriptButton).addClass('active');
        } else {
            $(this.subscriptButton).removeClass('active');
        }

        if (document.queryCommandState('superscript')) {
            $(this.superscriptButton).addClass('active');
        } else {
            $(this.superscriptButton).removeClass('active');
        }

        var fn = document.queryCommandValue('fontName');
        this.fontNameSelect.select(fn.replace('"', ''));

        var fs = document.queryCommandValue('fontSize');
        if (fs == '') {
            fs = '3';
        }
        this.fontSizeSelect.select(fs.replace('"', ''));
    }

    $(this.boldButton).on('click', function (e) {
        document.execCommand('bold');
        this.__richEditor.updateToolBox();
    });

    this.focusEditor = function () {
        this.editor.focus();
    }

    $(this.italicButton).on('click', function (e) {
        document.execCommand('italic');
        this.__richEditor.updateToolBox();
    });

    $(this.underlineButton).on('click', function (e) {
        document.execCommand('underline');
        this.__richEditor.updateToolBox();
    });

    $(this.strikeThroughButton).on('click', function (e) {
        document.execCommand('strikeThrough');
        this.__richEditor.updateToolBox();
    });

    $(this.justifyLeftButton).on('click', function (e) {
        document.execCommand('justifyLeft');
        this.__richEditor.updateToolBox();
    });

    $(this.justifyCenterButton).on('click', function (e) {
        document.execCommand('justifyCenter');
        this.__richEditor.updateToolBox();
    });

    $(this.justifyRightButton).on('click', function (e) {
        document.execCommand('justifyRight');
        this.__richEditor.updateToolBox();
    });

    $(this.justifyFullButton).on('click', function (e) {
        document.execCommand('justifyFull');
        this.__richEditor.updateToolBox();
    });

    $(this.insertUnorderedListButton).on('click', function (e) {
        document.execCommand('insertUnorderedList');
    });

    $(this.insertOrderedListButton).on('click', function (e) {
        document.execCommand('insertOrderedList');
    });

    $(this.indentButton).on('click', function (e) {
        document.execCommand('indent');
    });

    $(this.outdentButton).on('click', function (e) {
        document.execCommand('outdent');
    });

    $(this.createLinkButton).on('click', function (e) {
        var link = window.prompt('Nhập đường dẫn');
        var label = window.prompt('Nhập nhãn liên kết (nếu rỗng nhãn là đường dẫn)');
        if (label === null || label === '') {
            document.execCommand('insertHTML', false, '<a href="' + link + '">' + link + '</a>');
        } else {
            document.execCommand('insertHTML', false, '<a href="' + link + '">' + label + '</a>');
        }
    });

    $(this.unlinkButton).on('click', function (e) {
        document.execCommand('unlink');
    });

    $(this.insertImageButton).html('<label style="display: block; height: 100%; width: 100%"><input type="file" class="u-hidden"/></label>');

    $(this.insertImageButton).$('input[type="file"]')[0].__container = this;
    $(this.insertImageButton).$('input[type="file"]').on('change', function (e) {
        var reader = new FileReader();
        reader.__container = this.__container;
        reader.onloadend = function (e) {
            document.execCommand('insertImage', false, this.result);
            this.__container.updateContextMenuImage();
        }
        reader.readAsDataURL(this.files[0]);
    });

    this.updateContextMenuImage = function () {
        $(this.editor).$('img').oncontextmenu(function (e) {
            var p = new ImagePropertyPopup(this);
            p.show(e.x, e.y);
            e.preventDefault();
        });
    }

    $(this.insertImageButton).on('click', function (e) {
        //Big function

    });

    $(this.removeFormatButton).on('click', function (e) {
        document.execCommand('removeFormat');
    });

    $(this.subscriptButton).on('click', function (e) {
        document.execCommand('subscript');
        this.__richEditor.updateToolBox();
    });

    $(this.superscriptButton).on('click', function (e) {
        document.execCommand('superscript');
        this.__richEditor.updateToolBox();
    });

    $(this.removeButton).on('click', function (e) {
        $('div.editor :focus').remove();
    });

    this.row_1 = $.create('div');
    $(this.row_1).addClass('clearfix');
    $(this.toolbox).addEnd(this.row_1);

    this.boldButton.title = 'Tô đậm';
    this.italicButton.title = 'In nghiêng';
    this.underlineButton.title = 'Gạch chân';
    this.strikeThroughButton.title = 'Gạch xuyên';
    this.justifyLeftButton.title = 'Canh trái';
    this.justifyCenterButton.title = 'Canh giữa';
    this.justifyRightButton.title = 'Canh phải';
    this.justifyFullButton.title = 'Canh đều';
    this.insertUnorderedListButton.title = 'Định dạng danh sách không thứ tự';
    this.insertOrderedListButton.title = 'Định dạng danh sách có thứ tự';
    this.indentButton.title = 'Thụt đầu dòng';
    this.outdentButton.title = 'Nới ra đầu dòng';
    this.createLinkButton.title = 'Tạo liên kết';
    this.unlinkButton.title = 'Gỡ liên kết';
    this.insertImageButton.title = 'Chèn ảnh';
    this.removeFormatButton.title = 'Xóa tất cả định dạng';
    this.subscriptButton.title = 'Chữ nằm dưới';
    this.superscriptButton.title = 'Chữ nằm trên';
    this.removeButton.title = 'Xóa phần tử được focus';
    $(this.row_1).addEnd(this.boldButton).addEnd(this.italicButton).addEnd(this.underlineButton).addEnd(this.strikeThroughButton).addEnd(this.justifyLeftButton).addEnd(this.justifyCenterButton).addEnd(this.justifyRightButton).addEnd(this.justifyFullButton).addEnd(this.insertUnorderedListButton).addEnd(this.insertOrderedListButton).addEnd(this.indentButton).addEnd(this.outdentButton).addEnd(this.createLinkButton).addEnd(this.unlinkButton).addEnd(this.insertImageButton).addEnd(this.removeFormatButton).addEnd(this.subscriptButton).addEnd(this.superscriptButton).addEnd(this.removeButton);

    this.fontNameSelect = new RichEditorSelect();
    this.fontNameSelect.getContainer().title = 'Chọn phông chữ';
    this.fontNameSelect.setSelectedText('Arial');
    $(this.fontNameSelect.addItem('Arial', function (e) {
        document.execCommand('fontName', false, 'Arial');
    })).addClass('active').css('font-family', 'Arial');
    $(this.fontNameSelect.addItem('Tahoma', function (e) {
        document.execCommand('fontName', false, 'Tahoma');
    })).css('font-family', 'Tahoma');
    $(this.fontNameSelect.addItem('Verdana', function (e) {
        document.execCommand('fontName', false, 'Verdana');
    })).css('font-family', 'Verdana');
    $(this.fontNameSelect.addItem('Courier New', function (e) {
        document.execCommand('fontName', false, 'Courier New');
    })).css('font-family', 'Courier New');
    $(this.fontNameSelect.addItem('Times New Roman', function (e) {
        document.execCommand('fontName', false, 'Times New Roman');
    })).css('font-family', 'Times New Roman');
    $(this.fontNameSelect.addItem('serif', function (e) {
        document.execCommand('fontName', false, 'serif');
    })).css('font-family', 'serif');

    this.fontSizeSelect = new RichEditorSelect();
    this.fontSizeSelect.getContainer().title = 'Điều chỉnh kích thước font';
    this.fontSizeSelect.setSelectedText('4');
    this.fontSizeSelect.addItem('1', function (e) {
        document.execCommand('fontSize', false, 1);
    });

    this.fontSizeSelect.addItem('2', function (e) {
        document.execCommand('fontSize', false, 2);
    });

    this.fontSizeSelect.addItem('3', function (e) {
        document.execCommand('fontSize', false, 3);
    });

    $(this.fontSizeSelect.addItem('4', function (e) {
        document.execCommand('fontSize', false, 4);
    })).addClass('active');

    this.fontSizeSelect.addItem('5', function (e) {
        document.execCommand('fontSize', false, 5);
    });

    this.fontSizeSelect.addItem('6', function (e) {
        document.execCommand('fontSize', false, 6);
    });

    this.fontSizeSelect.addItem('7', function (e) {
        document.execCommand('fontSize', false, 7);
    });

    this.backColorPicker = new RichEditorColorPicker();
    this.backColorPicker.getContainer().title = 'Chọn màu nền cho chữ';

    this.backColorPicker.addColor('#fff');
    this.backColorPicker.addColor('#000');
    this.backColorPicker.addColor('#f00');
    this.backColorPicker.addColor('#0f0');
    this.backColorPicker.addColor('#00f');
    this.backColorPicker.addColor('#ff0');
    this.backColorPicker.addColor('#f0f');
    this.backColorPicker.addColor('#0ff');
    this.backColorPicker.addColor('#070');
    this.backColorPicker.addColor('#703');

    this.backColorPicker.setColor('#fff');

    this.backColorPicker.onchangecolor = function (c) {
        document.execCommand('backColor', false, c);
    }

    this.foreColorPicker = new RichEditorColorPicker();
    this.foreColorPicker.getContainer().title = 'Chọn màu cho chữ';

    this.foreColorPicker.addColor('#000');
    this.foreColorPicker.addColor('#f00');
    this.foreColorPicker.addColor('#0f0');
    this.foreColorPicker.addColor('#00f');
    this.foreColorPicker.addColor('#ff0');
    this.foreColorPicker.addColor('#f0f');
    this.foreColorPicker.addColor('#0ff');
    this.foreColorPicker.addColor('#070');
    this.foreColorPicker.addColor('#703');

    this.foreColorPicker.setColor('#000');

    this.foreColorPicker.onchangecolor = function (c) {
        document.execCommand('foreColor', false, c);
    }

    this.row_2 = $.create('div');
    $(this.toolbox).addEnd(this.row_2);
    $(this.row_2).addEnd(this.fontNameSelect.getContainer());
    $(this.row_2).addEnd(this.fontSizeSelect.getContainer());
    $(this.row_2).addEnd(this.backColorPicker.getContainer());
    $(this.row_2).addEnd(this.foreColorPicker.getContainer());

    this.content = $.create('div');
    $(this.content).addClass('content');
    $(this.content).addEnd(this.editor);
    $(this.content).addEnd(textarea);
    $(this.container).addEnd(this.toolbox);
    $(this.container).addEnd(this.content);

    pe.insertBefore(this.container, ne);

    this.editor.focus();
    document.execCommand('styleWithCSS', false, true);
    document.execCommand('fontSize', false, 4);
    document.execCommand('fontName', false, 'Arial');
    document.execCommand('formatBlock', false, 'div');
}


/*
 * 
 * 
 * Document click to remove all popup
 * 
 * 
 */

$(document).on('click', function (e) {
    $('[data-role="popup"][data-role-type="static"]').addClass('u-hidden');
    $('[data-role="popup"][data-role-type="dynamic"]').remove();
});