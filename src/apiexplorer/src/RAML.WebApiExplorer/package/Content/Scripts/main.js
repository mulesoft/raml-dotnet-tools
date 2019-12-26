let amfParser = require('parser');

window.onload = async function () {
    try {
        const url = document.getElementById('specurl').value;
        const type = document.getElementById('spectype').value;
        let data = await amfParser.amfParse(type, url);
        const apic = document.getElementById('api-console-app');
        apic.amf = JSON.parse(data);
        apic.selectedShape = 'summary';
        apic.selectedShapeType = 'summary';
    } catch (ex) {
        window.alert('Unable to parse AMF model ' + ex.toString());
    }
}