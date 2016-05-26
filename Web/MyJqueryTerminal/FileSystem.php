<?php

class File {
    public $name = "";
    public $content = "";
    public $path = "";
    
    public $owner;
    public $group;
    public $size;
    public $time;
    
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
    
    public function __construct(){
        
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

?>