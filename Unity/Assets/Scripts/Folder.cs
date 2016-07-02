using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Folder {

    public string name;
    public List<Folder> contentFolders;
    public List<File> contentFiles;
    public string path;
    public bool hidden = false;

    public bool[] ownerPermissions = { true, true, false };
    public bool[] groupPermissions = { true, true, false };
    public bool[] globalPermissions = { true, false, false };

    public string owner = "root";
    public string group = "root";
    public long size = 0;
    public string time = "Jun 8 11:27";

    public Folder parent;

    public Folder getParent()
    {
        return parent;
    }

    public void setParent(Folder f)
    {
        parent = f;
    }

    public Folder(string s, List<Folder> l, List<File> f, string p)
    {
        name = s;
        contentFolders = l;
        contentFiles = f;
        path = p;
    }

    public Folder(string n)
    {
        name = n;
		contentFolders = new List<Folder> ();
		contentFiles = new List<File> ();
    }

	public void add(bool f, string per, string o, string g, long s, string t, string n,bool h, string c)
    {

        if (f)
        {
			//Debug.Log ("Created folder, in: " + this.name);
            Folder tmpfold = new Folder(n);
            tmpfold.parent = this;
            tmpfold.owner = o;
            tmpfold.group = g;
            tmpfold.size = s;
            tmpfold.time = t;
            tmpfold.ownerPermissions = new bool[3] { per[0] == 'r', per[1] == 'w', per[2] == 'x' };
            tmpfold.groupPermissions = new bool[3] { per[3] == 'r', per[4] == 'w', per[5] == 'w' };
            tmpfold.globalPermissions = new bool[3] { per[6] == 'r', per[7] == 'w', per[8] == 'x' };
            tmpfold.path = this.path + name + "/";
			tmpfold.hidden = h;
            this.contentFolders.Add(tmpfold);
        }
        else
        {
			//Debug.Log ("Created file, in: " + this.name);
            File tmpfile = new File(n, c, this.path + name);
            tmpfile.owner = o;
            tmpfile.group = g;
            tmpfile.size = s;
            tmpfile.time = t;
            tmpfile.ownerPermissions = new bool[3] { per[0] == 'r', per[1] == 'w', per[2] == 'x' };
            tmpfile.groupPermissions = new bool[3] { per[3] == 'r', per[4] == 'w', per[5] == 'x' };
            tmpfile.globalPermissions = new bool[3] { per[6] == 'r', per[7] == 'w', per[8] == 'x' };
			tmpfile.hidden = h;
            this.contentFiles.Add(tmpfile);
        }

    }

    public void addFolder(string s)
    {
        contentFolders.Add(new Folder(s, new List<Folder>(), new List<File>(), path + name));
        contentFolders[contentFolders.Count - 1].setParent(this);
		//Debug.Log (contentFolders [contentFolders.Count - 1].parent.name);
    }

    public void addFile(string s, string c)
    {
        contentFiles.Add(new File(s, c, path + name));
    }

    public bool moveFile(string f)
    {
        // TODO


        return false;
    }

    public string printSize(bool h)
    {
        string ret = "";
        if (h)
        {
            // human readable

            if (size < 1024)
            {
                ret += size + "B";
            }
            else if (size < 1048576)
            {
                ret += (size / 1024) + "KB";
            }
            else if (size < 1073741824)
            {
                ret += (size / 1048576) + "MB";
            }
            else if (size < 1099511627776)
            {
                ret += (size / 1073741824) + "GB";
            }
            else
            {
                ret += (size / 1099511627776) + "TB";
            }

        }
        else
        {
            ret += size;
        }
        return ret;
    }

    public string printPermissions()
    {
        string ret = "d";

        if (ownerPermissions[0])
        {
            ret += "r";
        }
        else
        {
            ret += "-";
        }
        if (ownerPermissions[1])
        {
            ret += "w";
        }
        else
        {
            ret += "-";
        }
        if (ownerPermissions[2])
        {
            ret += "x";
        }
        else
        {
            ret += "-";
        }

        if (groupPermissions[0])
        {
            ret += "r";
        }
        else
        {
            ret += "-";
        }
        if (groupPermissions[1])
        {
            ret += "w";
        }
        else
        {
            ret += "-";
        }
        if (groupPermissions[2])
        {
            ret += "x";
        }
        else
        {
            ret += "-";
        }

        if (globalPermissions[0])
        {
            ret += "r";
        }
        else
        {
            ret += "-";
        }
        if (globalPermissions[1])
        {
            ret += "w";
        }
        else
        {
            ret += "-";
        }
        if (globalPermissions[2])
        {
            ret += "x";
        }
        else
        {
            ret += "-";
        }

        return ret;
    }

    public bool removeFolder(string f)
    {
        int tmp = -1;
        bool found = false;
        for (int i = 0; i < contentFolders.Count && !found; i++)
        {
            if (contentFolders[i].name == f)
            {
                tmp = i;
                found = true;
            }
        }
        if (found)
        {
            if (contentFolders[tmp].contentFolders.Count == 0 && contentFolders[tmp].contentFiles.Count == 0)
            {
                contentFolders.RemoveAt(tmp);
                return true;
            }
            else
            {
                Debug.Log("Can't remove a non empty folder");
                return false;
            }
        }
        else
        {
            Debug.Log("Couldn't find folder named: " + f);
            return false;
        }
    }

    public bool removeFile(string f)
    {
        bool found = false;
        for (int i = 0; i < contentFiles.Count && !found; i++)
        {
            if (contentFiles[i].name == f)
            {
                contentFiles.RemoveAt(i);
                found = true;
            }
        }
        if (found)
        {
            return true;
        }
        else
        {
            Debug.Log("Couldn't find file named: " + f);
            return false;
        }
    }
}
