var insert = {
    label: 'Insert',
    items: [
        {
            id: 'insert',
            label: 'Insert',
            items: ['link', 'table', 'ul', 'ol']
        }
    ]
}

var tools = {
    label: 'Tools',
    items: ['find', 'removeformat', 'fullscreen']
}

var customToolbar = {
    items: [
        insert,
        'style',
        'emphasis',
        'undo',
        tools
    ]
};

var instantiateTextbox = function () {
    textboxio.replace('textarea', {
        css: {
            styles: [
                { rule: 'p' },
                { rule: 'h4', text: 'Heading' },
            ],
            stylesheets: ['../css/styles.css']
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