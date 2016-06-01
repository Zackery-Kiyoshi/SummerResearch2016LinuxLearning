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
    
    checkPath(p){
        //console.log(p.length);
        if( p.length <= 0 || p[0] != null ) return true;
        var found = false;
        var next;
        
        for(var j=0; j < this.contentFolders.length; j++){
            console.log(p[0] == this.contentFolders[j].name);
            console.log( "'" + p[0] + "' : '" + this.contentFolders[j].name + "'");
            if(p[0] === this.contentFolders[j].name){
                found = true;
                next = this.contentFolders[j];
                break;
            }
        }
        
        if(found){
            //console.log(p);
            delete p[0];
            //console.log(p);
            return next.checkPath(p);
        }else return false
    }
    
    changeDir(p,cur){
        if( p.length <= 0 || p[0] != null ) return true;
        var found = false;
        var next;
        
        for(var j=0; j < this.contentFolders.length; j++){
            console.log(p[0] == this.contentFolders[j].name);
            console.log( "'" + p[0] + "' : '" + this.contentFolders[j].name + "'");
            if(p[0] === this.contentFolders[j].name){
                found = true;
                next = this.contentFolders[j];
                break;
            }
        }
        
        if(found){
            delete p[0];
            cur = next;
            return next.checkPath(p);
        }else return false
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
    
    getContents(){
        var ret = "";
        
        for(i=0; i< this.curFolder.contentFolders.length; i++){
            if(i == 0) ret += this.curFolder.contentFolders[i].name;
            else
                ret += "\t" + this.curFolder.contentFolders[i].name;
        }
        ret += "/";
        for(i=0; i< this.curFolder.contentFiles.length; i++){
            if(i == 0) ret += this.curFolder.contentFiles[i].name;
            else
                ret += "\t" + this.curFolder.contentFiles[i].name;
        }
        return ret;
    }
    
    checkPath(p){
        var tmp = p.split("/")
        if(p[0] == ''){
            // go to root
            return true
        }else if(p[0] == '/'){
            // absolute
            tmp = tmp.splice(0,1);
            return this.root.checkPath( tmp );
        }else{
            // relative
            return this.curFolder.checkPath( tmp );
        }
    }
    
    changeDir(p){
        var tmp = p.split("/")
        if(p[0] == ''){
            // go to root
            return true
        }else if(p[0] == '/'){
            // absolute
            tmp = tmp.splice(0,1);
            return this.root.checkPath( tmp );
        }else{
            // relative
            return this.curFolder.checkPath( tmp );
        }
    }
    
    currentPath(){
        return this.curFolder.name;
    }
    
}
