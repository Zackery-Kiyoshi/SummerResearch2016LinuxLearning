using UnityEngine;
using System.Collections;

public class File  {
    public string name;
    public string content = "";
    public string path;
	public bool hidden = false;

    public bool[] ownerPermissions = { true, true, false };
    public bool[] groupPermissions = { true, true, false };
    public bool[] globalPermissions = { true, false, false };

    public string owner = "root";
    public string group = "root";
    public long size = 0;
    public string time = "Jun 8 11:27";

    public File(string s, string c, string p)
    {
        name = s;
        content = c;
        path = p;
    }



    public void changeFile(string c)
    {
        content = c;
    }

    public void renameFile(string n)
    {
        name = n;
    }

    public string printSize(bool h)
    {
        // TODO
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
        // TODO
        string ret = "-";

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
}
