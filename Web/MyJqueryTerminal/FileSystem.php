<?php

class File {
    public $name = "";
    public $content = "";
    public $path = "";
    
    public $owner;
    public $group;
    public $size;
    public $time;
    
    public function __construct($n){
        $name = $n;
    }
    
    public function changeContents($newCont){
        $content = $newCont;
    }
    
}

class Folder {
    public $name = "";
    public $contentFolders = array();
    public $contentFiles = array();
    public $path = "";
    
    public $owner;
    public $group;
    public $size;
    public $time;
    //public $links;
    
    protected $parent;
    
    public function __construct($n,$p){
        $name = $n;
        $path = $p;
    }
    
    public function __construct($n){
        $name = $n;
    }
    
    public function addFolder($newFold){
        $this->contentFolders.array_push($newFold);
        $newFold->path = ($this->path) + ($this->name);
        ($newFold).changeParent($this);
    }
    
    public function addFile($newFile){
        $this->contentFiles.array_push($newFile);
        $newFile->path = ($this->path) + ($this->name);
    }
    
    public function rename($newName){
        $this->name = $newName;
    }
    
    public function changeParent($newPar){
        $this->parent = $newPar;
    }
    
    // takes an array of the path (split on /)
    public function checkPath($p, $prevFile){
        // checks that we made it through the path
        if($p.sizeof() <= 0) return true;
        // checksthat we didn't hit a file before the end
        if($prevFile) return true;
        
        $found = false;
        $fFile = true;
        $next;
        // check files
        $max = sizeof(contentFiles)
        for($i = 0; $i < $max; i++){
            if($contentFiles[i]->name == $p[0]){
                $found = true;
                $fFile = true;                
                break;
            }
        }
        if(!($found)){
            // check folders
            $1max = sizeof(contentFolders)
            for($i = 0; $i < $max; i++){
                if($contentFolders[i]->name == $p[0]){
                    $found = true;
                    $next = $contentFolders[i];     break;  
                }
            }
        }
        
        if($found){
            $max = sizeof($p);
            for($i = 0; $i < $max-1; i++){
                $p[i] = $p[i+1];
            }
            unset($p[sizeof($p)-1]);
            if($fFile){
                if(sizeof($p) <= 0){
                    return true;
                }else{
                    return false;
                }
            }else{
                return $next.checkPath($p,$fFile);
            }
        }else{
            return false;
        }
    }
}


class FileSystem {
    public $root;
    public $curFolder;
    public $test = "TESTING";
    
    public function __construct(){
        $root = new Folder("/","/");
        $curFolder = $root;
    }
    
    
    public function getPath(){
        return $fileSystem->$test;
        //return $fileSystem->curFolder->name;
    }
}

/*
fileSystem.root.addFile ("FirstTest0","TESTING0");
		fileSystem.root.addFile ("FirstTest1","TESTING1");
		fileSystem.root.addFile ("FirstTest2","TESTING2");
		fileSystem.root.addFile ("FirstTest3","TESTING3");

		fileSystem.root.addFolder ("FirstFolder");
		fileSystem.root.addFolder ("Downloads");
		fileSystem.root.addFolder ("Documents");
*/

var $fileSystem = new FileSystem();

$fileSystem->root.addFile(new File("FirstTest1"));
$fileSystem->root.addFile(new File("FirstTest2"));
$fileSystem->root.addFile(new File("FirstTest3"));

$fileSystem->root.addFolder(new Folder("FirstFolder"));
$fileSystem->root.addFolder(new Folder("Downloads"));
$fileSystem->root.addFolder(new Folder("Documents"));

/*
switch($_GET['ret']) { //Switch case for value of action
    case "pwd": if(!isset($fileSystem)){
        echo "NO";
    }
        else echo "FUCK";
    default: echo "IT NO THING"
*/

echo "SOMETHING";


?>