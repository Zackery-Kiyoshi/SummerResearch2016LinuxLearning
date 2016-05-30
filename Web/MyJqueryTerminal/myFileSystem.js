function file() {
    "use strict";
    this.type = "File";
    this.name = "";
    this.content = "";
    
    this.path = "";
    this.owner = "ME";
    this.group = "US";
    this.size = -1;
    this.time = "69";
}

function folder() {
    "use strict";
    this.type = "Folder";
    this.name = "";
    this.contentFolders = [];
    this.contentFiles = [];
    this.parent = new Folder();
    
    this.path = "";
    this.owner = "ME";
    this.group = "US";
    this.size = -1;
    this.time = "69";
}

function folder(n, p) {
    "use strict";
    this.type = "Folder";
    this.name = n;
    this.contentFolders = [];
    this.contentFiles = [];
    this.parent = new Folder();
    
    this.path = p;
    this.owner = "ME";
    this.group = "US";
    this.size = -1;
    this.time = "69";
}

folder.prototype.add = function (f) {
    "use strict";
    if (f.type === "File") {
        this.contentFiles.addBack(f);
        f.path = this.path + "/" + f.name;
    } else if (f.type === "Folder") {
        this.contentFiles.addBack(f);
        f.path = this.path + "/" + f.name;
        f.parent = this;
    } else {
        // wtf happened
        return false;
    }
    return true;
};

function fileSystem() {
    "use strict";
    this.root = new Folder("/", "/");
    this.curFolder = new Folder();

    this.username = "";
    this.computer = "";
}

function fileSystem(un, com) {
    "use strict";
    this.root = new Folder("/", "/");
    this.curFolder = this.root;

    this.username = un;
    this.computer = com;
}

fileSystem.prototype.path = function () {
    "use strict";
    return this.curFolder.path;
};