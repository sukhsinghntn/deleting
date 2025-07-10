window.initFieldDragDrop = (canvasSelector, dotnetHelper) => {
    const canvas = document.querySelector(canvasSelector);
    if (!canvas) return;
    if (canvas.dataset.dragInit === 'true') return;
    canvas.dataset.dragInit = 'true';

    let draggedType = null;

    document.querySelectorAll('.draggable-field').forEach(el => {
        if (el.dataset.dragInit === 'true') return;
        el.dataset.dragInit = 'true';
        el.addEventListener('dragstart', e => {
            draggedType = el.getAttribute('data-type');
            e.dataTransfer.setData('text/plain', draggedType);
        });
        el.addEventListener('dragend', () => draggedType = null);
    });

    canvas.addEventListener('dragover', e => {
        e.preventDefault();
    });

    canvas.addEventListener('drop', e => {
        e.preventDefault();
        const type = draggedType;
        draggedType = null;
        if (!type) return;

        const zone = e.target.closest('.row-dropzone');
        if (zone) {
            const insertIndex = parseInt(zone.getAttribute('data-insert'));
            dotnetHelper.invokeMethodAsync('AddRowFromDrop', type, insertIndex);
            return;
        }

        const rowEl = e.target.closest('.designer-row');
        let rowIndex = -1;
        let colIndex = -1;
        if (rowEl) {
            const rows = Array.from(canvas.querySelectorAll('.designer-row'));
            rowIndex = rows.indexOf(rowEl);
            const colEl = e.target.closest('[data-id]');
            if (colEl) {
                colIndex = parseInt(colEl.getAttribute('data-id'));
            }
        }
        dotnetHelper.invokeMethodAsync('AddFieldFromDrop', type, rowIndex, colIndex);
    });
}; 

// Backwards compatibility
window.initDragDrop = window.initFieldDragDrop;
