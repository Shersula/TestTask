document.getElementById("RootElement").addEventListener("click", GetDirectory);

function GetDirectory(event) {
    fetch(document.location.protocol + "//" + document.location.host + "/FileManager/GetDirectory?code=" + this.getAttribute('key'), {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    }).then(response => response.json()).then(obj => {
        AddChildElemet(this.closest('div'), obj)
    });
}

function AddChildElemet(root, childList) {
    if (root.querySelector('svg').getAttribute("class") == "bi bi-caret-down")
    {
        root.querySelector('svg').setAttribute("class", "bi bi-caret-right");
        root.querySelector('svg').querySelector('path').setAttribute("d", "M6 12.796V3.204L11.481 8 6 12.796zm.659.753 5.48-4.796a1 1 0 0 0 0-1.506L6.66 2.451C6.011 1.885 5 2.345 5 3.204v9.592a1 1 0 0 0 1.659.753z");

        childList = [...root.children];
        childList.forEach(item => {
            if (item.nodeName == "DIV") item.remove();
        })
    }
    else
    {
        root.querySelector('svg').setAttribute("class", "bi bi-caret-down");
        root.querySelector('svg').querySelector('path').setAttribute("d", "M3.204 5h9.592L8 10.481 3.204 5zm-.753.659 4.796 5.48a1 1 0 0 0 1.506 0l4.796-5.48c.566-.647.106-1.659-.753-1.659H3.204a1 1 0 0 0-.753 1.659z");

        if (childList != null) {
            childList.forEach(item => {
                el = document.createElement('div');
                root.appendChild(el);

                btn = document.createElement('svg');
                btn.setAttribute("xmlns", "http://www.w3.org/2000/svg");
                btn.setAttribute("width", "16");
                btn.setAttribute("height", "16");
                btn.setAttribute("fill", "currentColor");
                btn.setAttribute("class", "bi bi-caret-right");
                btn.setAttribute("viewBox", "0 0 16 16");
                btn.setAttribute("key", item.hash);

                path = document.createElement('path');
                path.setAttribute("d", "M6 12.796V3.204L11.481 8 6 12.796zm.659.753 5.48-4.796a1 1 0 0 0 0-1.506L6.66 2.451C6.011 1.885 5 2.345 5 3.204v9.592a1 1 0 0 0 1.659.753z");
                btn.appendChild(path);
                el.appendChild(btn);

                el.appendChild(document.createTextNode(item.name + " " + item.weight + " МБ"));
                el.style.marginLeft = "2em";
                btn.addEventListener("click", GetDirectory);
            })
        }
    }
    
}