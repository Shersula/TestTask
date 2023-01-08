document.getElementById("RootElement").addEventListener("click", TriangleButton);
document.getElementById("CreateFolderButton").addEventListener("click", AddFolder);
document.getElementById("CreateFileButton").addEventListener("click", AddFile);

function GetDirectory(key, root) {
    fetch(document.location.protocol + "//" + document.location.host + "/FileManager/GetDirectory?ParentCode=" + key, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    }).then(response => response.json()).then(obj => {
        UpdateChildElemet(root, obj)
    });
}

function TriangleButton(event) {
    root = this.parentElement;
    if (root.querySelector('i').getAttribute("class") == "bi bi-caret-down") ClearChild(root);
    else GetDirectory(this.getAttribute('key'), root);
}

function ClearChild(root) {
    root.querySelector('i').setAttribute("class", "bi bi-caret-right");

    childList = [...root.children];
    childList.forEach(item => {
        if (item.nodeName == "DIV") item.remove();
    })
}

function UpdateChildElemet(root, childList) {
    ClearChild(root);
    if (childList != null) {
        root.querySelector('i').setAttribute("class", "bi bi-caret-down");
        childList.forEach(item => {
            el = document.createElement('div');
            root.appendChild(el);

            if (item.isAFile == true) {
                icon = document.createElement('i');
                icon.setAttribute("class", "bi bi-file-earmark");
                icon.setAttribute("key", item.hash);
                el.appendChild(icon);
            }
            else {
                btn = document.createElement('i');
                btn.setAttribute("class", "bi bi-caret-right");
                btn.setAttribute("key", item.hash);
                el.appendChild(btn);
                btn.addEventListener("click", TriangleButton);

                icon = document.createElement('i');
                icon.setAttribute("class", "bi bi-folder");
                el.appendChild(icon);
            }

            el.appendChild(document.createTextNode(item.name + " (" + item.weight + " МБ)"));
            el.style.marginLeft = "1em";

            if (item.isAFile == true) {
                btn = document.createElement('button');
                btn.setAttribute("type", "button");
                btn.setAttribute("class", "btn btn-danger btn-sm");
                el.appendChild(btn);

                icon = document.createElement('i');
                icon.setAttribute("class", "bi bi-file-earmark-minus");
                btn.appendChild(icon);
                btn.addEventListener("click", RemoveElement);
            }
            else {
                btn = document.createElement('button');
                btn.setAttribute("type", "button");
                btn.setAttribute("class", "btn btn-success btn-sm");
                btn.setAttribute("data-bs-toggle", "modal");
                btn.setAttribute("data-bs-target", "#AddFolder");
                el.appendChild(btn);

                icon = document.createElement('i');
                icon.setAttribute("class", "bi bi-folder-plus");
                btn.appendChild(icon);
                btn.addEventListener("click", AddFolderModal);
                ///////////////////////////////////////////////////////////////////////
                btn = document.createElement('button');
                btn.setAttribute("type", "button");
                btn.setAttribute("class", "btn btn-success btn-sm");
                btn.setAttribute("data-bs-toggle", "modal");
                btn.setAttribute("data-bs-target", "#AddFile");
                el.appendChild(btn);

                icon = document.createElement('i');
                icon.setAttribute("class", "bi bi-file-earmark-plus");
                btn.appendChild(icon);
                btn.addEventListener("click", AddFileModal);
                ///////////////////////////////////////////////////////////////////////
                btn = document.createElement('button');
                btn.setAttribute("type", "button");
                btn.setAttribute("class", "btn btn-danger btn-sm");
                el.appendChild(btn);

                icon = document.createElement('i');
                icon.setAttribute("class", "bi bi-folder-minus");
                btn.appendChild(icon);
                btn.addEventListener("click", RemoveElement);
            }

        })
    }
}

function RemoveElement(event) {
    root = this.parentElement;
    key = root.querySelector("i[key]").getAttribute("key");
    fetch(document.location.protocol + "//" + document.location.host + "/FileManager/RemoveElement?ElementCode=" + key, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' }
    }).then(response => {
        if (response.ok == true) GetDirectory(root.parentElement.querySelector("i[key]").getAttribute("key"), root.parentElement);
    });
}

function AddFileModal(event) {
    document.getElementById("AddFile").setAttribute("key", this.parentElement.querySelector("i[key]").getAttribute("key"));
}

function AddFile(event) {
    if (document.getElementById("ErrMsg") != null) document.getElementById("ErrMsg").remove();

    if (document.getElementById("FileNameInput").value.length <= 0) {
        console.log("+");
        err = document.createElement('p');
        err.appendChild(document.createTextNode("Вы не ввели название файла"));
        err.setAttribute("class", "text-danger");
        err.setAttribute("id", "ErrMsg");
        document.getElementById("AddFile").querySelector(".modal-body").appendChild(err);
    }
    else if (document.getElementById("FileWeightInput").value <= 0) {
        err = document.createElement('p');
        err.appendChild(document.createTextNode("Неверное значение размера"));
        err.setAttribute("class", "text-danger");
        err.setAttribute("id", "ErrMsg");
        document.getElementById("AddFile").querySelector(".modal-body").appendChild(err);
    }
    else {
        key = document.getElementById("AddFile").getAttribute("key");
        fetch("FileManager/AddElement", {
            method: 'POST',
            body: JSON.stringify({
                ParentCode: key,
                Name: document.getElementById("FileNameInput").value,
                isAFile: true,
                Weight: document.getElementById("FileWeightInput").value
            }),
            headers: { 'Content-Type': 'application/json' }
        }).then(response => {
            if (response.ok == true) {
                modal = bootstrap.Modal.getInstance(document.getElementById('AddFile'));
                document.getElementById("FileNameInput").value = "";
                document.getElementById("FileWeightInput").value = "";
                modal.hide();
                GetDirectory(key, document.querySelector("i[key=\"" + key + "\"]").parentElement);
            }
        });
    }
}

function AddFolderModal(event) {
    document.getElementById("AddFolder").setAttribute("key", this.parentElement.querySelector("i[key]").getAttribute("key"));
}

function AddFolder(event) {
    if (document.getElementById("ErrMsg") != null) document.getElementById("ErrMsg").remove();

    if (document.getElementById("FolderNameInput").value.length <= 0) {
        err = document.createElement('p');
        err.appendChild(document.createTextNode("Вы ничего не ввели"));
        err.setAttribute("class", "text-danger");
        err.setAttribute("id", "ErrMsg");
        document.getElementById("AddFolder").querySelector(".modal-body").appendChild(err);
    }
    else {
        key = document.getElementById("AddFolder").getAttribute("key");
        fetch("FileManager/AddElement", {
            method: 'POST',
            body: JSON.stringify({
                ParentCode: key,
                Name: document.getElementById("FolderNameInput").value,
                isAFile: false,
                Weight: 0
            }),
            headers: { 'Content-Type': 'application/json' }
        }).then(response => {
            if (response.ok == true)
            {
                modal = bootstrap.Modal.getInstance(document.getElementById('AddFolder'));
                document.getElementById("FolderNameInput").value = "";
                modal.hide();
                GetDirectory(key, document.querySelector("i[key=\"" + key + "\"]").parentElement);
            }
        });
    }
}