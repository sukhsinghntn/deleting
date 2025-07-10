export function downloadTableCsv(tableId, filename) {
    const table = document.getElementById(tableId);
    if (!table) return;
    const rows = Array.from(table.querySelectorAll('tr')).map(r =>
        Array.from(r.querySelectorAll('th,td')).map(c => '"' + c.innerText.replace(/"/g,'""') + '"').join(',')
    );
    const csv = rows.join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(link.href);
};
window.downloadTableCsv = downloadTableCsv;

export async function exportTableToPdf(elementId, filename) {
    const el = document.getElementById(elementId);
    if (!el) return;
    if (!window.jspdf || !window.html2canvas) {
        alert('PDF export is unavailable.');
        return;
    }
    const canvas = await html2canvas(el, { useCORS: true });
    const data = canvas.toDataURL('image/png');
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF({
        orientation: canvas.width > canvas.height ? 'l' : 'p',
        unit: 'px',
        format: [canvas.width, canvas.height]
    });
    doc.addImage(data, 'PNG', 0, 0, canvas.width, canvas.height);
    doc.save(filename);
};
window.exportTableToPdf = exportTableToPdf;

export async function exportTableToDocx(elementId, filename) {
    const el = document.getElementById(elementId);
    if (!el) return;
    if (!window.docx || !window.html2canvas) {
        alert('DOCX export is unavailable.');
        return;
    }
    const canvas = await html2canvas(el, { useCORS: true });
    const dataUrl = canvas.toDataURL('image/png');
    const base64 = dataUrl.split(',')[1];
    const bytes = Uint8Array.from(atob(base64), c => c.charCodeAt(0));
    const { Document, Packer, Paragraph, ImageRun } = window.docx;
    const doc = new Document({
        sections: [{
            children: [
                new Paragraph({
                    children: [
                        new ImageRun({ data: bytes, transformation: { width: canvas.width, height: canvas.height } })
                    ]
                })
            ]
        }]
    });
    Packer.toBlob(doc).then(blob => {
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    });
};
window.exportTableToDocx = exportTableToDocx;

export function exportTableToExcel(tableId, filename) {
    const table = document.getElementById(tableId);
    if (!table) return;
    if (!window.XLSX) {
        alert('Excel export is unavailable.');
        return;
    }
    const wb = XLSX.utils.table_to_book(table, { sheet: 'Sheet1' });
    const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([wbout], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(link.href);
};
window.exportTableToExcel = exportTableToExcel;

export function printElement(elementId) {
    const el = document.getElementById(elementId);
    if (!el) return;
    const frame = document.createElement('iframe');
    frame.style.position = 'fixed';
    frame.style.right = '0';
    frame.style.bottom = '0';
    frame.style.width = '0';
    frame.style.height = '0';
    frame.style.border = '0';
    document.body.appendChild(frame);
    const doc = frame.contentWindow.document;
    doc.open();
    doc.write('<html><head><title>Print</title>');
    doc.write('<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" />');
    doc.write('</head><body>');
    doc.write(el.outerHTML);
    doc.write('</body></html>');
    doc.close();
    frame.contentWindow.focus();
    frame.contentWindow.print();
    setTimeout(() => document.body.removeChild(frame), 100);
};
window.printElement = printElement;
