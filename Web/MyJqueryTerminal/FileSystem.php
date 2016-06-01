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
    
    public function changePath($p){
        $this->path = $p;
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
    
    public function changePath($p){
        $this->path = $p;
    }
    
    public function addFolder($newFold){
        array_push($this->contentFolders, $newFold);
        $newFold->changePath( ($this->path) . ($newFold->name) ."/" );
        $newFold->changeParent($this);
    }
    
    public function addFileN($newFile){
        $tmp = new File($newFile);
        array_push($this->contentFiles, new File($newFile));
        //$index = 
        $this->contentFiles[sizeof($this->contentFiles )-1]->name = ($newFile);
        //echo nl2br( ($this->contentFiles[sizeof($this->contentFiles )-1]->name) . "\n");
        $this->contentFiles[sizeof($this->contentFiles )-1]->path = ($this->path) . ($this->name) . "/";
    }
    
    public function addFile($newFile){
        array_push($this->contentFiles, $newFile);
        $newFile->changePath( ($this->path) . ($this->name) );
        //$newFile->path = ($this->path) . ($this->name);
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
    public function checkPath($p){
        // checks that we made it through the path
        if(sizeof($p) <= 0) return true;
        // checksthat we didn't hit a file before the end
        
        /*
        $max = sizeof($p);
        for($i = 0; $i < $max; $i++){
            echo nl2br( $i . " : " . $p[$i] . ",");
        }
        */
        
        $found = false;
        $fFile = false;
        $next;
        // check files
        /*
        $max = sizeof($this->contentFiles);
        for($i = 0; $i < $max; $i++){
            //echo " :" . $this->contentFiles[$i] . "=?=" . $p[0] . ": ";
            if(strcmp($this->contentFiles[$i]->name, $p[0]) == 0){
                $found = true;
                $fFile = true;                
                break;
            }
        }
        */
        
        if(!($found)){
            // check folders
            $max = sizeof($this->contentFolders);
            for($i = 0; $i < $max; $i++){
                //console.error(" :" . $this->contentFolders[$i] . "=?=" . $p[0] . ": ");
                if( strcmp($this->contentFolders[$i]->name, $p[0]) == 0){
                    $found = true;
                    $next = $this->contentFolders[$i];
                    
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
                return $next->checkPath($p,$fFile);
            }
        }else{
            return false;
        }
    }
    
    // takes an array of the path (split on /)
    public function getFolder($p, &$cur){
        // checks that we made it through the path
        if(sizeof($p) <= 0) return true;
        // checksthat we didn't hit a file before the end
        if($prevFile)return true;
        
        $found = false;
        $next;
        
        if(!($found)){
            // check folders
            $max = sizeof($this->contentFolders);
            for($i = 0; $i < $max; $i++){
                //console.error(" :" . $this->contentFolders[$i] . "=?=" . $p[0] . ": ");
                if( strcmp($this->contentFolders[$i]->name, $p[0]) == 0){
                    $found = true;
                    $next = $this->contentFolders[$i];
                    //echo "Found Next: " . $this->contentFolders[$i]->name;
                    $cur = $this->contentFolders[$i];
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
                    //$cur = $next;
                    return;
                }else{
                    return false;
                }
            }else{
                return $next->getFolder($p,$fFile);
            }
        }else{
            return false;
        }
    }
    
}


/*
class FileSystem {
    
    
    
    public $root;
    public $_SESSION["curFolder"] = new Folder();
    public $test = "TESTING";
    
    public function __construct(){
        $this->root = new Folder("/");
        $this->root->path = "/";
        $this->_SESSION["curFolder"] = $this->root;
    }
    
    public function getPath(){
        //echo $this->curFolder->path;
        return $this->_SESSION["curFolder"]->path;
        //return $fileSystem->curFolder->name;
    }
    
    public function checkPath($p){
        //echo "Path:" . $p . ": ";
        if($p[0] == ''){
            return true;
        }else if($p[0] == '/'){
            $p[0] = '';
            return $this->root->checkPath(explode("/",$p, PHP_INT_MAX ));
        }else{
            return $this->_SESSION["curFolder"]->checkPath(explode("/",$p, PHP_INT_MAX ));
        }
    }
    
    public function changePath($p){
        if($p[0] == ''){
            $this->_SESSION["curFolder"] = $root;
        }else if($p[0] == '/'){
            $p[0] = '';
            $this->root->getFolder(explode("/",$p, PHP_INT_MAX ), $this->curFolder);
        }else{
            $this->_SESSION["curFolder"]->getFolder(explode("/",$p, PHP_INT_MAX ), $this->curFolder);
        }
    }
    
    public function printCurFolderContents(){
        $ret = "";
        
        $max = sizeof($this->_SESSION["curFolder"]->contentFolders);
        for($i = 0; $i < $max; $i++){
            if($i == 0){
                $tmp = $this->_SESSION["curFolder"]->contentFolders[$i]->name;
                $ret .= $tmp;
            } 
                else $ret .= "\t" . $this->$_SESSION["curFolder"]->contentFolders[$i]->name;
        }
        $ret .= "/";
        $max = sizeof($this->$_SESSION["curFolder"]->contentFiles);
        for($i = 0; $i < $max; $i++){
            if($i == 0){
                $ret .= $this->_SESSION["curFolder"]->contentFiles[$i]->name . "";
            } 
            else $ret .= "\t" . $this->_SESSION["curFolder"]->contentFiles[$i]->name . "";
        }
        //echo $ret;
        return $ret;
    }
}
*/


session_start();

$root = new Folder("/");
$root->path = "/";
$_SESSION["curFolder"] = $root;

$root->addFile(new File("FirstTest1"));
$root->addFileN(("FirstTest2"));
$root->addFileN(("FirstTest3"));
$root->addFolder(new Folder("FirstFolder"));
$root->addFolder(new Folder("Downloads"));
$root->addFolder(new Folder("Documents"));

/*
echo nl2br("0: " . $fileSystem->curFolder->contentFolders[0]->name . "\n");
echo nl2br("1: " . $fileSystem->curFolder->contentFolders[1]->name . "\n");
echo nl2br("2: " . $fileSystem->curFolder->contentFolders[2]->name . "\n");
echo nl2br("3: " . $fileSystem->curFolder->contentFolders[3]->name . "\n");

echo nl2br("0: " . $fileSystem->curFolder->contentFiles[0]->name . "\n");
echo nl2br("1: " . $fileSystem->curFolder->contentFiles[1]->name . "\n");
echo nl2br("2: " . $fileSystem->curFolder->contentFiles[2]->name . "\n");
echo nl2br("3: " . $fileSystem->curFolder->contentFiles[3]->name . "\n");


echo nl2br($fileSystem->printCurFolderContents() . "\n");
*/

switch($_GET['ret']) { //Switch case for value of action
    case "pwd": 
        echo $_SESSION["curFolder"]->path;
        break;
    case "cd":
        $path = $_GET['path'];
        $isValid = false;
        if($p[0] == ''){
            $isValid = true;
        }else if($p[0] == '/'){
            $p[0] = '';
            $isValid = $this->root->checkPath(explode("/",$p, PHP_INT_MAX ));
        }else{
            $isValid = $this->_SESSION["curFolder"]->checkPath(explode("/",$p, PHP_INT_MAX ));
        }
        
        if($isValid){
            //echo "Good path";
            
            if($p[0] == ''){
                $_SESSION["curFolder"] = $root;
            }else if($p[0] == '/'){
                $p[0] = '';
                $root->getFolder(explode("/",$p, PHP_INT_MAX ), $this->curFolder);
            }else{
                $_SESSION["curFolder"]->getFolder(explode("/",$p, PHP_INT_MAX ), $this->curFolder);
            }
            
            echo $_SESSION["curFolder"]->path;
        }else{
            echo "-bash: cd: help: No such file or directory (" . $path . ")";
        }
        break;
    case "ls":
        
        $ret = "";
        $max = sizeof($_SESSION["curFolder"]->contentFolders);
        for($i = 0; $i < $max; $i++){
            if($i == 0){
                $tmp = $_SESSION["curFolder"]->contentFolders[$i]->name;
                $ret .= $tmp;
            } 
                else $ret .= "\t" . $_SESSION["curFolder"]->contentFolders[$i]->name;
        }
        $ret .= "/";
        $max = sizeof($_SESSION["curFolder"]->contentFiles);
        for($i = 0; $i < $max; $i++){
            if($i == 0){
                $ret .= $_SESSION["curFolder"]->contentFiles[$i]->name . "";
            } 
            else $ret .= "\t" . $_SESSION["curFolder"]->contentFiles[$i]->name . "";
        }
        
        echo $ret;
        break;
    default: 
        echo "IT NO THING";
}
//echo $_GET['ret'];
//echo "SOMETHING";


?>