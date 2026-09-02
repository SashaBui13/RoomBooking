let serviceIndex = 0;

function addServiceField() {
    const container = document.getElementById('services-list');
    if (!container) return;

    const row = document.createElement('div');
    row.className = 'form-row service-entry-row';
    row.style.marginBottom = '10px';

    row.innerHTML = `
        <div class="form-group" style="flex: 2;">
            <input name="Services[${serviceIndex}].Name" class="form-control" placeholder="Назва (наприклад, Проєктор)" required />
        </div>
        <div class="form-group" style="flex: 1;">
            <input name="Services[${serviceIndex}].Price" type="number" step="0.01" min="0" class="form-control" placeholder="Ціна (грн)" required />
        </div>
        <div class="form-group" style="flex: 0;">
            <button type="button" class="btn btn-danger" onclick="removeServiceField(this)">✕</button>
        </div>
    `;

    container.appendChild(row);
    serviceIndex++;
}

function removeServiceField(buttonElement) {
    const row = buttonElement.closest('.service-entry-row');
    if (row) {
        row.remove();
    }
}