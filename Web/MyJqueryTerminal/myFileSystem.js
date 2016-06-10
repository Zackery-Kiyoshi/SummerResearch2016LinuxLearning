class File {
    constructor(n) {
        this.type = "File";
        this.name = n;
        this.content = "";

        this.path = "";
        this.owner = "ME";
        this.group = "US";
        this.size = -1;
        this.time = "69";
    }
}

class Folder {
    constructor(n) {
        this.type = "Folder";
        this.name = n;
        this.contentFolders = [];
        this.contentFiles = [];
        this.parent = null;

        this.path = "";
        this.owner = "ME";
        this.group = "US";
        this.size = -1;
        this.time = "69";
    }

    add(f) {
        if (f.type === "File") {
            this.contentFiles.push(f);
            f.path = this.path + "/" + f.name;
        } else if (f.type === "Folder") {
            this.contentFolders.push(f);
            f.path = this.path + "/" + f.name;
            f.parent = this;
        } else {
            // wtf happened
            return false;
        }
        return true;
    }

    checkPath(p, z) {
        //console.log(p.length);
        if (p.length >= z) {
            //console.log(this.name);
            return true;
        }
        var found = false;
        var next;
        if (p[z] == "..") {
            z++;
            return this.parent.checkPath(p, z);
        } else {
            for (var j = 0; j < this.contentFolders.length; j++) {
                console.log(p[z] == this.contentFolders[j].name);
                //console.log( "'" + p[z] + "' : '" + this.contentFolders[j].name + "'");
                if (p[z] === this.contentFolders[j].name) {
                    found = true;
                    next = this.contentFolders[j];
                    break;
                }
            }

            if (found) {
                //console.log(p);
                //delete p[0];
                z++;
                //console.log(p);
                return next.checkPath(p, z);
            } else return false
        }
    }

    changeDir(p, z) {
        if (p.length <= z) {
            //console.log("return: " + this.name);
            return this;
        }
        var found = false;
        var next;
        if (p[z] == "..") {
            z++;
            return this.parent.changeDir(p, z);
        } else {
            for (var j = 0; j < this.contentFolders.length; j++) {
                //console.log(p[z] == this.contentFolders[j].name);
                //console.log( "'" + p[z] + "' : '" + this.contentFolders[j].name + "'");
                if (p[z] == this.contentFolders[j].name) {
                    found = true;
                    next = this.contentFolders[j];
                    break;
                }
            }

            if (found) {
                //delete p[0];
                z++;
                //cur = next;
                return next.changeDir(p, z);
            } else return null;
        }
    }
}


class FileSystem {
    constructor(un, com) {
        this.root = new Folder("/");
        this.root.path = "/";
        this.curFolder = this.root;

        this.username = un;
        this.computer = com;
    }

    path() {
        console.log("'" + this.curFolder.path + "'");
        return this.curFolder.path;
    }

    getContents() {
        var ret = "";

        for (i = 0; i < this.curFolder.contentFolders.length; i++) {
            if (i == 0) ret += this.curFolder.contentFolders[i].name;
            else
                ret += "\t" + this.curFolder.contentFolders[i].name;
        }
        ret += "/";
        for (i = 0; i < this.curFolder.contentFiles.length; i++) {
            if (i == 0) ret += this.curFolder.contentFiles[i].name;
            else
                ret += "\t" + this.curFolder.contentFiles[i].name;
        }
        return ret;
    }

    checkPath(p) {
        var tmp = p.split("/")
        if (p[0] == '') {
            // go to root
            return true
        } else if (p[0] == '/') {
            // absolute
            tmp = tmp.splice(0, 1);
            return this.root.checkPath(tmp, 0);
        } else {
            // relative
            return this.curFolder.checkPath(tmp, 0);
        }
    }

    changeDir(p) {
        var tmp = p.split("/")
        if (p[0] == '') {
            // go to root
            return true
        } else if (p[0] == '/') {
            // absolute
            tmp = tmp.splice(0, 1);
            var tmpFolder = this.root.changeDir(tmp, 0);
            this.curFolder = tmpFolder;
            return;
        } else {
            // relative
            //console.log(p);
            var tmpFolder = this.curFolder.changeDir(tmp, 0);
            //console.log(tmpFolder.name);
            this.curFolder = tmpFolder;
            return;
        }
    }

    currentPath() {
        return this.curFolder.name;
    }

}
