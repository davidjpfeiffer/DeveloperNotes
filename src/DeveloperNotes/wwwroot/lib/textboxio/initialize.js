var customToolbar = {
    items: [
        {
            label: 'Group',
            items: [
                {
                    id: 'insert',
                    label: 'Insert',
                    items: ['link', 'table']
                }
            ]
        },
        'style',
        'emphasis',
        'align',
        {
            label: 'Group',
            items: ['undo', 'redo']
        },
        {
            label: 'Group',
            items: ['removeformat', 'fullscreen']
        }
    ]
};

var instantiateTextbox = function () {
    textboxio.replace('textarea', {
        css: {
            styles: [
                { rule: 'p' },
                { rule: 'h3', text: 'Heading' },
            ],
            stylesheets: ['/lib/textboxio/resources/css/styles.css']
        },
        paste: {
            style: 'clean'
        },
        spelling: {}, //http://spelling.ephox.com/
        ui: { toolbar: customToolbar }
    })};

instantiateTextbox();

var getEditorContent = function () {
    var editors = textboxio.get('#textbox');
    var editor = editors[0];
    return editor.content.get();
};