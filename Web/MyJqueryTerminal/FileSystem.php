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
        $this -> name = $n;
    }
    
    public function changeContents($newCont){
        $content = $newCont;
    }
    
    public function getName(){
        return $this->name;
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
    
    public function __construct($n){
        $this->name = $n;
    }
    
    public function getName(){
        return $this->name;
    }
    
    public function addFolder($newFold){
        array_push($this->contentFolders, $newFold);
        $newFold->path = ($this->path) + ($this->name);
        $newFold->changeParent($this);
    }
    
    public function addFileN($newFile){
        $tmp = new File($newFile);
        array_push($this->contentFiles, new File($newFile));
        //$index = 
        $this->contentFiles[sizeof($this->contentFiles )-1]->name = ($newFile);
        //echo nl2br( ($this->contentFiles[sizeof($this->contentFiles )-1]->name) . "\n");
        $this->contentFiles[sizeof($this->contentFiles )-1]->path = ($this->path) + ($this->name);
    }
    
    public function addFile($newFile){
        array_push($this->contentFiles, $newFile);
        $newFile->path = ($this->path) + ($this->name);
        //echo "'" . $newFile->name . "' : ";
        //echo nl2br( "'" . ($this->contentFiles[sizeof($this->contentFiles )-1]->name) . "'\n");
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
        if(sizeof($p) <= 0) return true;
        // checksthat we didn't hit a file before the end
        if($prevFile)return true;
        
        $found = false;
        $fFile = true;
        $next;
        // check files
        $max = sizeof($contentFiles);
        for($i = 0; $i < $max; $i++){
            if($contentFiles[i]->name == $p[0]){
                $found = true;
                $fFile = true;                
                break;
            }
        }
        if(!($found)){
            // check folders
            $max = sizeof($contentFolders);
            for($i = 0; $i < $max; $i++){
                if($contentFolders[i]->name == $p[0]){
                    $found = true;
                    $next = $contentFolders[i];
                    break;  
                }
            }
        }
        
        if($found){
            $max = sizeof($p);
            for($i = 0; $i < $max-1; $i++){
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
    
//    /*
    public function __construct(){
        $this->root = new Folder("/");
        $this->root->path = "/";
        $this->curFolder = $this->root;
    }
//    */
    
    public function getPath(){
        //echo $this->curFolder->path;
        return $this->curFolder->path;
        //return $fileSystem->curFolder->name;
    }
    
    public function checkPath($p){
        if($p[0] == '/'){
            $p[0] = '';
            return $this->root.checkPath(explode("/",$p, PHP_INT_MAX ),false);
        }else{
            return $this->curFolder->checkPath(explode("/",$p, PHP_INT_MAX ),false);
        }
    }
    
    public function printCurFolderContents(){
        $ret = "";
        
        echo "TEST";
echo nl2br("0: " . $this->curFolder->contentFolders[0]->name . "\n");
echo nl2br("1: " . $this->curFolder->contentFolders[1]->name . "\n");
echo nl2br("2: " . $this->curFolder->contentFolders[2]->name . "\n");
echo nl2br("3: " . $this->curFolder->contentFolders[3]->name . "\n");

echo nl2br("0: " . $this->curFolder->contentFiles[0]->name . "\n");
echo nl2br("1: " . $this->curFolder->contentFiles[1]->name . "\n");
echo nl2br("2: " . $this->curFolder->contentFiles[2]->name . "\n");
echo nl2br("3: " . $this->curFolder->contentFiles[3]->name . "\n");
        
        $max = sizeof($this->curFolder->contentFolders);
        for($i = 0; $i < $max; $i++){
            if($i == 0){
                $tmp = $this->curFolder->contentFolders[$i]->name;
                $ret .= $tmp;
            } 
                else $ret .= " & " . $this->curFolder->contentFolders[$i]->name;
        }
        $max = sizeof($this->curFolder->contentFiles);
        for($i = 0; $i < $max; $i++){
            $ret .= " & " . $this->curFolder->contentFiles[$i]->name . "";
        }
        //echo $ret;
        return $ret;
    }
}



$fileSystem = new FileSystem();
$tmpfile = new File("FirstTest1");
$fileSystem->root->addFile($tmpfile);
$fileSystem->root->addFileN(("FirstTest2"));
$fileSystem->root->addFileN(("FirstTest3"));
$tmpfold = new Folder("FirstFolder");
$fileSystem->root->addFolder($tmpfold);
$fileSystem->root->addFolder(new Folder("Downloads"));
$fileSystem->root->addFolder(new Folder("Documents"));


echo nl2br("0: " . $fileSystem->curFolder->contentFolders[0]->name . "\n");
echo nl2br("1: " . $fileSystem->curFolder->contentFolders[1]->name . "\n");
echo nl2br("2: " . $fileSystem->curFolder->contentFolders[2]->name . "\n");
echo nl2br("3: " . $fileSystem->curFolder->contentFolders[3]->name . "\n");

echo nl2br("0: " . $fileSystem->curFolder->contentFiles[0]->name . "\n");
echo nl2br("1: " . $fileSystem->curFolder->contentFiles[1]->name . "\n");
echo nl2br("2: " . $fileSystem->curFolder->contentFiles[2]->name . "\n");
echo nl2br("3: " . $fileSystem->curFolder->contentFiles[3]->name . "\n");


echo nl2br($fileSystem->printCurFolderContents() . "\n");


switch($_GET['ret']) { //Switch case for value of action
    case "pwd": 
        $tmp = $fileSystem->getPath();
        echo $tmp;
        break;
    case "cd":
        $path = $_GET['path'];
        if($fileSystem->checkPath($path)){
            echo "Good path";
        }
        else{
            echo "-bash: cd: help: No such file or directory";
        }
        break;
    case "ls":
        $fileSystem->printCurFolderContents();
        break;
    default: 
        echo "IT NO THING";
}
//echo $_GET['ret'];
//echo "SOMETHING";


?>