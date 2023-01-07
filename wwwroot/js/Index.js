document.getElementById("RootElement").addEventListener("click", GetDirectory);
document.getElementById("CreateFolderButton").addEventListener("click", AddFolder);

function GetDirectory(event) {
    fetch(document.location.protocol + "//" + document.location.host + "/FileManager/GetDirectory?code=" + this.getAttribute('key'), {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    }).then(response => response.json()).then(obj => {
        AddChildElemet(this.closest('div'), obj)
    });
}

function AddChildElemet(root, childList) {
    if (root.querySelector('i').getAttribute("class") == "bi bi-caret-down")
    {
        root.querySelector('i').setAttribute("class", "bi bi-caret-right");

        childList = [...root.children];
        childList.forEach(item => {
            if (item.nodeName == "DIV") item.remove();
        })
    }
    else
    {
        root.querySelector('i').setAttribute("class", "bi bi-caret-down");
   

        if (childList != null) {
            childList.forEach(item => {
                el = document.createElement('div');
                root.appendChild(el);

                btn = document.createElement('i');
                btn.setAttribute("class", "bi bi-caret-right");
                btn.setAttribute("key", item.hash);
                el.appendChild(btn);

                if (item.isAFile == true) {
                    icon = document.createElement('i');
                    icon.setAttribute("class", "bi bi-file-earmark");
                    el.appendChild(icon);
                }
                else {
                    icon = document.createElement('i');
                    icon.setAttribute("class", "bi bi-folder");
                    el.appendChild(icon);
                }

                el.appendChild(document.createTextNode(item.name + " (" + item.weight + " МБ)"));
                el.style.marginLeft = "1em";
                btn.addEventListener("click", GetDirectory);

                if (item.isAFile == true) {
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
                }

            })
        }
    }
    
}

function AddFolderModal(event) {
    document.getElementById("AddFolder").setAttribute("key", this.closest('div').querySelector("i[key]").getAttribute("key"));
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
        console.log(document.getElementById("AddFolder").getAttribute("key"));
        console.log(document.getElementById("FolderNameInput").value);

        fetch("FileManager/AddElement", {
            method: 'POST',
            body: JSON.stringify({
                ParentCode: document.getElementById("AddFolder").getAttribute("key"),
                Name: document.getElementById("FolderNameInput").value,
                isAFile: false
            }),
            headers: { 'Content-Type': 'application/json' }
        }).then(response => {
            if (response.ok == true) {
                modal = bootstrap.Modal.getInstance(document.getElementById('AddFolder'));
                modal.hide();
                document.querySelector("i[key=\"" + document.getElementById("AddFolder").getAttribute("key") + "\"]").dispatchEvent(new Event("click"));
            }
        });
    }
}