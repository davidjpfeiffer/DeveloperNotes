CKEDITOR.editorConfig = function (config) {
    config.toolbarGroups = [
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'about', groups: ['about'] }
    ];

    config.removeButtons = 'Subscript,Cut,Copy,Paste,PasteText,PasteFromWord,HorizontalRule,Outdent,Indent';
    config.autoGrow_bottomSpace = 25;
    config.autoGrow_minHeight = 400;
    config.autoGrow_onStartup = true;
    config.format_tags = 'p;h3';
    config.forcePasteAsPlainText = true;
};