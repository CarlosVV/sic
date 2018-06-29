namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Configuration;
    using Infrastructure.Core.Dependencies;
    using System.Runtime.Serialization;

    #endregion

    public static class CommonHelper
    {
        #region Private Members

        /// <summary>
        /// Cache key of asset hash
        /// </summary>
        public const string ResourceOptimizationHashCacheKey = "Nagnoi.SiC.ResourceOptimizationHash";

        /// <summary>
        /// Asset Hash field
        /// </summary>
        private static string assetHash = "1.0.0.0";

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a cache key
        /// </summary>
        /// <param name="setName">Set name</param>
        /// <returns>Cache key formatted</returns>
        public static string GetCacheKey(string setName)
        {
            return string.Format(
                "Nagnoi.SiC.NonSecureResourceHandler.{0}.{1}.{2}",
                SystemSettings.ApplicationId, 
                setName, 
                assetHash);
        }

        /// <summary>
        /// Finds a control recursive
        /// </summary>
        /// <typeparam name="T">Control class</typeparam>
        /// <param name="controls">Input control collection</param>
        /// <returns>Found control</returns>
        public static T FindControlRecursive<T>(ControlCollection controls) where T : class
        {
            T found = default(T);

            if (controls != null && controls.Count > 0)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i] is T)
                    {
                        found = controls[i] as T;
                        break;
                    }
                    else
                    {
                        found = FindControlRecursive<T>(controls[i].Controls);
                        if (found != null)
                        {
                            break;
                        }
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// recursively finds a child control of the specified parent.
        /// </summary>
        /// <param name="control">Web control</param>
        /// <param name="id">Control identifier</param>
        /// <returns>Returns the web control</returns>
        public static Control FindControlRecursive(this Control control, string id)
        {
            if (control == null)
            {
                return null;
            }

            // try to find the control at the current level
            Control foundControl = control.FindControl(id);

            if (foundControl == null)
            {
                // search the children
                foreach (Control child in control.Controls)
                {
                    foundControl = FindControlRecursive(child, id);

                    if (foundControl != null)
                    {
                        break;
                    }
                }
            }

            return foundControl;
        }

        /// <summary>
        /// Convert enumeration for front-end
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;

            char[] letters = str.ToCharArray();

            foreach (char c in letters)
            {
                if (c.ToString() != c.ToString().ToLower())
                {
                    result += " " + c.ToString();
                }
                else
                {
                    result += c.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Selects scan
        /// </summary>
        /// <param name="list">DropdownList control</param>
        /// <param name="item">Value to select</param>
        public static void SelectListItem(ListControl list, object value)
        {
            if (list.Items.Count != 0)
            {
                var selectedItem = list.SelectedItem;
                if (selectedItem != null)
                {
                    selectedItem.Selected = false;
                }

                if (value != null)
                {
                    selectedItem = list.Items.FindByValue(value.ToString());
                    if (selectedItem != null)
                    {
                        selectedItem.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Reset any control to default item
        /// </summary>
        /// <param name="parent">Parent Control</param>
        public static void ResetFormControlValues(Control parent)
        {
            foreach (Control childControl in parent.Controls)
            {
                if (childControl.Controls.Count > 0)
                {
                    ResetFormControlValues(childControl);
                }
                else
                {
                    switch (childControl.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)childControl).Text = string.Empty;
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)childControl).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)childControl).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)childControl).SelectedIndex = -1;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the image url from file extension
        /// </summary>
        /// <param name="fileExtension">File extension</param>
        /// <returns>Return the url</returns>
        public static string GetImageSpriteFromFileExtension(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return string.Empty;
            }

            fileExtension = fileExtension.ToLowerInvariant();

            if (fileExtension.StartsWith("."))
            {
                fileExtension = fileExtension.Substring(1, fileExtension.Length - 1);
            }

            switch (fileExtension.ToLowerInvariant())
            {
                case "docx":
                case "doc":
                    return "sprite sprite-ms-word-small";
                case "xlsx":
                case "xls":
                    return "sprite sprite-ms-excel-small";
                case "pdf":
                    return "sprite sprite-pdficon-small";
                case "txt":
                    return "sprit sprite-txticon-small";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Locks screen after postback
        /// </summary>
        /// <param name="control">Web control instance</param>
        /// <param name="message">Resource Key of Message shown while screen is locked</param>
        /// <param name="page">Page where the JavaScript will be registered</param>
        /// <param name="confirmationMessage">Resource Key of popup Confirmation Message shown before the screen is locked (screen will be locked only if user clicks OK). If null or empty, no confirmation message is shown.</param>
        public static void LockScreenAfterPostBack(WebControl control, string message, Page page, string confirmationMessage)
        {
            AddLockScreenScript(page);

            string scriptText = string.Empty;
            if (string.IsNullOrEmpty(confirmationMessage))
            {
                scriptText = string.Format("skm_LockScreen('{0}');", message.Replace("'", "\'"));
            }
            else
            {
                scriptText = string.Format("var confirmed = confirm('{0}'); if (confirmed) skm_LockScreen('{1}'); return confirmed;", confirmationMessage.Replace("'", "\'"), message.Replace("'", "\'"));
            }

            control.Attributes.Add("onclick", scriptText);
        }

        /// <summary>
        /// Locks screen after postback
        /// </summary>
        /// <param name="control">Web control instance</param>
        /// <param name="message">Resource Key of Message shown while screen is locked</param>
        /// <param name="page">Page where the JavaScript will be registered</param>
        public static void LockScreenAfterPostBack(WebControl control, string message, Page page)
        {
            LockScreenAfterPostBack(control, message, page, confirmationMessage: null);
        }

        /// <summary>
        /// Compares two byte arrays
        /// </summary>
        /// <param name="a1">First array</param>
        /// <param name="a2">Second array</param>
        /// <returns>Returns true or false whether both arrays match</returns>
        public static bool CompareArray(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
            {
                return false;
            }

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Includes a CSS link to any page
        /// </summary>
        /// <param name="name">Absolute path</param>
        /// <returns>Returns the html link</returns>
        public static string IncludeCss(string name)
        {
            return string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\"></link>", GetCacheAwareUrl(name, false));
        }

        /// <summary>
        /// Includes a Javascript reference to any page
        /// </summary>
        /// <param name="name">Absolute path</param>
        /// <returns>Returns the html reference</returns>
        public static string IncludeJs(string name)
        {
            return string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", GetCacheAwareUrl(name, false));
        }

        /// <summary>
        /// Gets the image src
        /// </summary>
        /// <param name="name">Absolute path</param>
        /// <returns>Return the image src</returns>
        public static string GetImageSrc(string name)
        {
            return GetCacheAwareUrl(name, false);
        }

        /// <summary>
        /// Gets the mime type from file extension
        /// </summary>
        /// <param name="extension">File extension</param>
        /// <returns>Returns the mime type</returns>
        public static string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            switch (extension)
            {
                #region Big freaking list of mime types

                // combination of values from Windows 7 Registry and 
                // from C:\Windows\System32\inetsrv\config\applicationHost.config
                // some added, including .7z and .dat

                case ".323": return "text/h323";
                case ".3g2": return "video/3gpp2";
                case ".3gp": return "video/3gpp";
                case ".3gp2": return "video/3gpp2";
                case ".3gpp": return "video/3gpp";
                case ".7z": return "application/x-7z-compressed";
                case ".aa": return "audio/audible";
                case ".AAC": return "audio/aac";
                case ".aaf": return "application/octet-stream";
                case ".aax": return "audio/vnd.audible.aax";
                case ".ac3": return "audio/ac3";
                case ".aca": return "application/octet-stream";
                case ".accda": return "application/msaccess.addin";
                case ".accdb": return "application/msaccess";
                case ".accdc": return "application/msaccess.cab";
                case ".accde": return "application/msaccess";
                case ".accdr": return "application/msaccess.runtime";
                case ".accdt": return "application/msaccess";
                case ".accdw": return "application/msaccess.webapplication";
                case ".accft": return "application/msaccess.ftemplate";
                case ".acx": return "application/internet-property-stream";
                case ".AddIn": return "text/xml";
                case ".ade": return "application/msaccess";
                case ".adobebridge": return "application/x-bridge-url";
                case ".adp": return "application/msaccess";
                case ".ADT": return "audio/vnd.dlna.adts";
                case ".ADTS": return "audio/aac";
                case ".afm": return "application/octet-stream";
                case ".ai": return "application/postscript";
                case ".aif": return "audio/x-aiff";
                case ".aifc": return "audio/aiff";
                case ".aiff": return "audio/aiff";
                case ".air": return "application/vnd.adobe.air-application-installer-package+zip";
                case ".amc": return "application/x-mpeg";
                case ".application": return "application/x-ms-application";
                case ".art": return "image/x-jg";
                case ".asa": return "application/xml";
                case ".asax": return "application/xml";
                case ".ascx": return "application/xml";
                case ".asd": return "application/octet-stream";
                case ".asf": return "video/x-ms-asf";
                case ".ashx": return "application/xml";
                case ".asi": return "application/octet-stream";
                case ".asm": return "text/plain";
                case ".asmx": return "application/xml";
                case ".aspx": return "application/xml";
                case ".asr": return "video/x-ms-asf";
                case ".asx": return "video/x-ms-asf";
                case ".atom": return "application/atom+xml";
                case ".au": return "audio/basic";
                case ".avi": return "video/x-msvideo";
                case ".axs": return "application/olescript";
                case ".bas": return "text/plain";
                case ".bcpio": return "application/x-bcpio";
                case ".bin": return "application/octet-stream";
                case ".bmp": return "image/bmp";
                case ".c": return "text/plain";
                case ".cab": return "application/octet-stream";
                case ".caf": return "audio/x-caf";
                case ".calx": return "application/vnd.ms-office.calx";
                case ".cat": return "application/vnd.ms-pki.seccat";
                case ".cc": return "text/plain";
                case ".cd": return "text/plain";
                case ".cdda": return "audio/aiff";
                case ".cdf": return "application/x-cdf";
                case ".cer": return "application/x-x509-ca-cert";
                case ".chm": return "application/octet-stream";
                case ".class": return "application/x-java-applet";
                case ".clp": return "application/x-msclip";
                case ".cmx": return "image/x-cmx";
                case ".cnf": return "text/plain";
                case ".cod": return "image/cis-cod";
                case ".config": return "application/xml";
                case ".contact": return "text/x-ms-contact";
                case ".coverage": return "application/xml";
                case ".cpio": return "application/x-cpio";
                case ".cpp": return "text/plain";
                case ".crd": return "application/x-mscardfile";
                case ".crl": return "application/pkix-crl";
                case ".crt": return "application/x-x509-ca-cert";
                case ".cs": return "text/plain";
                case ".csdproj": return "text/plain";
                case ".csh": return "application/x-csh";
                case ".csproj": return "text/plain";
                case ".css": return "text/css";
                case ".csv": return "application/octet-stream";
                case ".cur": return "application/octet-stream";
                case ".cxx": return "text/plain";
                case ".dat": return "application/octet-stream";
                case ".datasource": return "application/xml";
                case ".dbproj": return "text/plain";
                case ".dcr": return "application/x-director";
                case ".def": return "text/plain";
                case ".deploy": return "application/octet-stream";
                case ".der": return "application/x-x509-ca-cert";
                case ".dgml": return "application/xml";
                case ".dib": return "image/bmp";
                case ".dif": return "video/x-dv";
                case ".dir": return "application/x-director";
                case ".disco": return "text/xml";
                case ".dll": return "application/x-msdownload";
                case ".dll.config": return "text/xml";
                case ".dlm": return "text/dlm";
                case ".doc": return "application/msword";
                case ".docm": return "application/vnd.ms-word.document.macroEnabled.12";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".dot": return "application/msword";
                case ".dotm": return "application/vnd.ms-word.template.macroEnabled.12";
                case ".dotx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case ".dsp": return "application/octet-stream";
                case ".dsw": return "text/plain";
                case ".dtd": return "text/xml";
                case ".dtsConfig": return "text/xml";
                case ".dv": return "video/x-dv";
                case ".dvi": return "application/x-dvi";
                case ".dwf": return "drawing/x-dwf";
                case ".dwp": return "application/octet-stream";
                case ".dxr": return "application/x-director";
                case ".eml": return "message/rfc822";
                case ".emz": return "application/octet-stream";
                case ".eot": return "application/octet-stream";
                case ".eps": return "application/postscript";
                case ".etl": return "application/etl";
                case ".etx": return "text/x-setext";
                case ".evy": return "application/envoy";
                case ".exe": return "application/octet-stream";
                case ".exe.config": return "text/xml";
                case ".fdf": return "application/vnd.fdf";
                case ".fif": return "application/fractals";
                case ".filters": return "Application/xml";
                case ".fla": return "application/octet-stream";
                case ".flr": return "x-world/x-vrml";
                case ".flv": return "video/x-flv";
                case ".fsscript": return "application/fsharp-script";
                case ".fsx": return "application/fsharp-script";
                case ".generictest": return "application/xml";
                case ".gif": return "image/gif";
                case ".group": return "text/x-ms-group";
                case ".gsm": return "audio/x-gsm";
                case ".gtar": return "application/x-gtar";
                case ".gz": return "application/x-gzip";
                case ".h": return "text/plain";
                case ".hdf": return "application/x-hdf";
                case ".hdml": return "text/x-hdml";
                case ".hhc": return "application/x-oleobject";
                case ".hhk": return "application/octet-stream";
                case ".hhp": return "application/octet-stream";
                case ".hlp": return "application/winhlp";
                case ".hpp": return "text/plain";
                case ".hqx": return "application/mac-binhex40";
                case ".hta": return "application/hta";
                case ".htc": return "text/x-component";
                case ".htm": return "text/html";
                case ".html": return "text/html";
                case ".htt": return "text/webviewhtml";
                case ".hxa": return "application/xml";
                case ".hxc": return "application/xml";
                case ".hxd": return "application/octet-stream";
                case ".hxe": return "application/xml";
                case ".hxf": return "application/xml";
                case ".hxh": return "application/octet-stream";
                case ".hxi": return "application/octet-stream";
                case ".hxk": return "application/xml";
                case ".hxq": return "application/octet-stream";
                case ".hxr": return "application/octet-stream";
                case ".hxs": return "application/octet-stream";
                case ".hxt": return "text/html";
                case ".hxv": return "application/xml";
                case ".hxw": return "application/octet-stream";
                case ".hxx": return "text/plain";
                case ".i": return "text/plain";
                case ".ico": return "image/x-icon";
                case ".ics": return "application/octet-stream";
                case ".idl": return "text/plain";
                case ".ief": return "image/ief";
                case ".iii": return "application/x-iphone";
                case ".inc": return "text/plain";
                case ".inf": return "application/octet-stream";
                case ".inl": return "text/plain";
                case ".ins": return "application/x-internet-signup";
                case ".ipa": return "application/x-itunes-ipa";
                case ".ipg": return "application/x-itunes-ipg";
                case ".ipproj": return "text/plain";
                case ".ipsw": return "application/x-itunes-ipsw";
                case ".iqy": return "text/x-ms-iqy";
                case ".isp": return "application/x-internet-signup";
                case ".ite": return "application/x-itunes-ite";
                case ".itlp": return "application/x-itunes-itlp";
                case ".itms": return "application/x-itunes-itms";
                case ".itpc": return "application/x-itunes-itpc";
                case ".IVF": return "video/x-ivf";
                case ".jar": return "application/java-archive";
                case ".java": return "application/octet-stream";
                case ".jck": return "application/liquidmotion";
                case ".jcz": return "application/liquidmotion";
                case ".jfif": return "image/pjpeg";
                case ".jnlp": return "application/x-java-jnlp-file";
                case ".jpb": return "application/octet-stream";
                case ".jpe": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".jpg": return "image/jpeg";
                case ".js": return "application/x-javascript";
                case ".jsx": return "text/jscript";
                case ".jsxbin": return "text/plain";
                case ".latex": return "application/x-latex";
                case ".library-ms": return "application/windows-library+xml";
                case ".lit": return "application/x-ms-reader";
                case ".loadtest": return "application/xml";
                case ".lpk": return "application/octet-stream";
                case ".lsf": return "video/x-la-asf";
                case ".lst": return "text/plain";
                case ".lsx": return "video/x-la-asf";
                case ".lzh": return "application/octet-stream";
                case ".m13": return "application/x-msmediaview";
                case ".m14": return "application/x-msmediaview";
                case ".m1v": return "video/mpeg";
                case ".m2t": return "video/vnd.dlna.mpeg-tts";
                case ".m2ts": return "video/vnd.dlna.mpeg-tts";
                case ".m2v": return "video/mpeg";
                case ".m3u": return "audio/x-mpegurl";
                case ".m3u8": return "audio/x-mpegurl";
                case ".m4a": return "audio/m4a";
                case ".m4b": return "audio/m4b";
                case ".m4p": return "audio/m4p";
                case ".m4r": return "audio/x-m4r";
                case ".m4v": return "video/x-m4v";
                case ".mac": return "image/x-macpaint";
                case ".mak": return "text/plain";
                case ".man": return "application/x-troff-man";
                case ".manifest": return "application/x-ms-manifest";
                case ".map": return "text/plain";
                case ".master": return "application/xml";
                case ".mda": return "application/msaccess";
                case ".mdb": return "application/x-msaccess";
                case ".mde": return "application/msaccess";
                case ".mdp": return "application/octet-stream";
                case ".me": return "application/x-troff-me";
                case ".mfp": return "application/x-shockwave-flash";
                case ".mht": return "message/rfc822";
                case ".mhtml": return "message/rfc822";
                case ".mid": return "audio/mid";
                case ".midi": return "audio/mid";
                case ".mix": return "application/octet-stream";
                case ".mk": return "text/plain";
                case ".mmf": return "application/x-smaf";
                case ".mno": return "text/xml";
                case ".mny": return "application/x-msmoney";
                case ".mod": return "video/mpeg";
                case ".mov": return "video/quicktime";
                case ".movie": return "video/x-sgi-movie";
                case ".mp2": return "video/mpeg";
                case ".mp2v": return "video/mpeg";
                case ".mp3": return "audio/mpeg";
                case ".mp4": return "video/mp4";
                case ".mp4v": return "video/mp4";
                case ".mpa": return "video/mpeg";
                case ".mpe": return "video/mpeg";
                case ".mpeg": return "video/mpeg";
                case ".mpf": return "application/vnd.ms-mediapackage";
                case ".mpg": return "video/mpeg";
                case ".mpp": return "application/vnd.ms-project";
                case ".mpv2": return "video/mpeg";
                case ".mqv": return "video/quicktime";
                case ".ms": return "application/x-troff-ms";
                case ".msi": return "application/octet-stream";
                case ".mso": return "application/octet-stream";
                case ".mts": return "video/vnd.dlna.mpeg-tts";
                case ".mtx": return "application/xml";
                case ".mvb": return "application/x-msmediaview";
                case ".mvc": return "application/x-miva-compiled";
                case ".mxp": return "application/x-mmxp";
                case ".nc": return "application/x-netcdf";
                case ".nsc": return "video/x-ms-asf";
                case ".nws": return "message/rfc822";
                case ".ocx": return "application/octet-stream";
                case ".oda": return "application/oda";
                case ".odc": return "text/x-ms-odc";
                case ".odh": return "text/plain";
                case ".odl": return "text/plain";
                case ".odp": return "application/vnd.oasis.opendocument.presentation";
                case ".ods": return "application/oleobject";
                case ".odt": return "application/vnd.oasis.opendocument.text";
                case ".one": return "application/onenote";
                case ".onea": return "application/onenote";
                case ".onepkg": return "application/onenote";
                case ".onetmp": return "application/onenote";
                case ".onetoc": return "application/onenote";
                case ".onetoc2": return "application/onenote";
                case ".orderedtest": return "application/xml";
                case ".osdx": return "application/opensearchdescription+xml";
                case ".p10": return "application/pkcs10";
                case ".p12": return "application/x-pkcs12";
                case ".p7b": return "application/x-pkcs7-certificates";
                case ".p7c": return "application/pkcs7-mime";
                case ".p7m": return "application/pkcs7-mime";
                case ".p7r": return "application/x-pkcs7-certreqresp";
                case ".p7s": return "application/pkcs7-signature";
                case ".pbm": return "image/x-portable-bitmap";
                case ".pcast": return "application/x-podcast";
                case ".pct": return "image/pict";
                case ".pcx": return "application/octet-stream";
                case ".pcz": return "application/octet-stream";
                case ".pdf": return "application/pdf";
                case ".pfb": return "application/octet-stream";
                case ".pfm": return "application/octet-stream";
                case ".pfx": return "application/x-pkcs12";
                case ".pgm": return "image/x-portable-graymap";
                case ".pic": return "image/pict";
                case ".pict": return "image/pict";
                case ".pkgdef": return "text/plain";
                case ".pkgundef": return "text/plain";
                case ".pko": return "application/vnd.ms-pki.pko";
                case ".pls": return "audio/scpls";
                case ".pma": return "application/x-perfmon";
                case ".pmc": return "application/x-perfmon";
                case ".pml": return "application/x-perfmon";
                case ".pmr": return "application/x-perfmon";
                case ".pmw": return "application/x-perfmon";
                case ".png": return "image/png";
                case ".pnm": return "image/x-portable-anymap";
                case ".pnt": return "image/x-macpaint";
                case ".pntg": return "image/x-macpaint";
                case ".pnz": return "image/png";
                case ".pot": return "application/vnd.ms-powerpoint";
                case ".potm": return "application/vnd.ms-powerpoint.template.macroEnabled.12";
                case ".potx": return "application/vnd.openxmlformats-officedocument.presentationml.template";
                case ".ppa": return "application/vnd.ms-powerpoint";
                case ".ppam": return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
                case ".ppm": return "image/x-portable-pixmap";
                case ".pps": return "application/vnd.ms-powerpoint";
                case ".ppsm": return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
                case ".ppsx": return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".pptm": return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".prf": return "application/pics-rules";
                case ".prm": return "application/octet-stream";
                case ".prx": return "application/octet-stream";
                case ".ps": return "application/postscript";
                case ".psc1": return "application/PowerShell";
                case ".psd": return "application/octet-stream";
                case ".psess": return "application/xml";
                case ".psm": return "application/octet-stream";
                case ".psp": return "application/octet-stream";
                case ".pub": return "application/x-mspublisher";
                case ".pwz": return "application/vnd.ms-powerpoint";
                case ".qht": return "text/x-html-insertion";
                case ".qhtm": return "text/x-html-insertion";
                case ".qt": return "video/quicktime";
                case ".qti": return "image/x-quicktime";
                case ".qtif": return "image/x-quicktime";
                case ".qtl": return "application/x-quicktimeplayer";
                case ".qxd": return "application/octet-stream";
                case ".ra": return "audio/x-pn-realaudio";
                case ".ram": return "audio/x-pn-realaudio";
                case ".rar": return "application/octet-stream";
                case ".ras": return "image/x-cmu-raster";
                case ".rat": return "application/rat-file";
                case ".rc": return "text/plain";
                case ".rc2": return "text/plain";
                case ".rct": return "text/plain";
                case ".rdlc": return "application/xml";
                case ".resx": return "application/xml";
                case ".rf": return "image/vnd.rn-realflash";
                case ".rgb": return "image/x-rgb";
                case ".rgs": return "text/plain";
                case ".rm": return "application/vnd.rn-realmedia";
                case ".rmi": return "audio/mid";
                case ".rmp": return "application/vnd.rn-rn_music_package";
                case ".roff": return "application/x-troff";
                case ".rpm": return "audio/x-pn-realaudio-plugin";
                case ".rqy": return "text/x-ms-rqy";
                case ".rtf": return "application/rtf";
                case ".rtx": return "text/richtext";
                case ".ruleset": return "application/xml";
                case ".s": return "text/plain";
                case ".safariextz": return "application/x-safari-safariextz";
                case ".scd": return "application/x-msschedule";
                case ".sct": return "text/scriptlet";
                case ".sd2": return "audio/x-sd2";
                case ".sdp": return "application/sdp";
                case ".sea": return "application/octet-stream";
                case ".searchConnector-ms": return "application/windows-search-connector+xml";
                case ".setpay": return "application/set-payment-initiation";
                case ".setreg": return "application/set-registration-initiation";
                case ".settings": return "application/xml";
                case ".sgimb": return "application/x-sgimb";
                case ".sgml": return "text/sgml";
                case ".sh": return "application/x-sh";
                case ".shar": return "application/x-shar";
                case ".shtml": return "text/html";
                case ".sit": return "application/x-stuffit";
                case ".sitemap": return "application/xml";
                case ".skin": return "application/xml";
                case ".sldm": return "application/vnd.ms-powerpoint.slide.macroEnabled.12";
                case ".sldx": return "application/vnd.openxmlformats-officedocument.presentationml.slide";
                case ".slk": return "application/vnd.ms-excel";
                case ".sln": return "text/plain";
                case ".slupkg-ms": return "application/x-ms-license";
                case ".smd": return "audio/x-smd";
                case ".smi": return "application/octet-stream";
                case ".smx": return "audio/x-smd";
                case ".smz": return "audio/x-smd";
                case ".snd": return "audio/basic";
                case ".snippet": return "application/xml";
                case ".snp": return "application/octet-stream";
                case ".sol": return "text/plain";
                case ".sor": return "text/plain";
                case ".spc": return "application/x-pkcs7-certificates";
                case ".spl": return "application/futuresplash";
                case ".src": return "application/x-wais-source";
                case ".srf": return "text/plain";
                case ".SSISDeploymentManifest": return "text/xml";
                case ".ssm": return "application/streamingmedia";
                case ".sst": return "application/vnd.ms-pki.certstore";
                case ".stl": return "application/vnd.ms-pki.stl";
                case ".sv4cpio": return "application/x-sv4cpio";
                case ".sv4crc": return "application/x-sv4crc";
                case ".svc": return "application/xml";
                case ".swf": return "application/x-shockwave-flash";
                case ".t": return "application/x-troff";
                case ".tar": return "application/x-tar";
                case ".tcl": return "application/x-tcl";
                case ".testrunconfig": return "application/xml";
                case ".testsettings": return "application/xml";
                case ".tex": return "application/x-tex";
                case ".texi": return "application/x-texinfo";
                case ".texinfo": return "application/x-texinfo";
                case ".tgz": return "application/x-compressed";
                case ".thmx": return "application/vnd.ms-officetheme";
                case ".thn": return "application/octet-stream";
                case ".tif": return "image/tiff";
                case ".tiff": return "image/tiff";
                case ".tlh": return "text/plain";
                case ".tli": return "text/plain";
                case ".toc": return "application/octet-stream";
                case ".tr": return "application/x-troff";
                case ".trm": return "application/x-msterminal";
                case ".trx": return "application/xml";
                case ".ts": return "video/vnd.dlna.mpeg-tts";
                case ".tsv": return "text/tab-separated-values";
                case ".ttf": return "application/octet-stream";
                case ".tts": return "video/vnd.dlna.mpeg-tts";
                case ".txt": return "text/plain";
                case ".u32": return "application/octet-stream";
                case ".uls": return "text/iuls";
                case ".user": return "text/plain";
                case ".ustar": return "application/x-ustar";
                case ".vb": return "text/plain";
                case ".vbdproj": return "text/plain";
                case ".vbk": return "video/mpeg";
                case ".vbproj": return "text/plain";
                case ".vbs": return "text/vbscript";
                case ".vcf": return "text/x-vcard";
                case ".vcproj": return "Application/xml";
                case ".vcs": return "text/plain";
                case ".vcxproj": return "Application/xml";
                case ".vddproj": return "text/plain";
                case ".vdp": return "text/plain";
                case ".vdproj": return "text/plain";
                case ".vdx": return "application/vnd.ms-visio.viewer";
                case ".vml": return "text/xml";
                case ".vscontent": return "application/xml";
                case ".vsct": return "text/xml";
                case ".vsd": return "application/vnd.visio";
                case ".vsi": return "application/ms-vsi";
                case ".vsix": return "application/vsix";
                case ".vsixlangpack": return "text/xml";
                case ".vsixmanifest": return "text/xml";
                case ".vsmdi": return "application/xml";
                case ".vspscc": return "text/plain";
                case ".vss": return "application/vnd.visio";
                case ".vsscc": return "text/plain";
                case ".vssettings": return "text/xml";
                case ".vssscc": return "text/plain";
                case ".vst": return "application/vnd.visio";
                case ".vstemplate": return "text/xml";
                case ".vsto": return "application/x-ms-vsto";
                case ".vsw": return "application/vnd.visio";
                case ".vsx": return "application/vnd.visio";
                case ".vtx": return "application/vnd.visio";
                case ".wav": return "audio/wav";
                case ".wave": return "audio/wav";
                case ".wax": return "audio/x-ms-wax";
                case ".wbk": return "application/msword";
                case ".wbmp": return "image/vnd.wap.wbmp";
                case ".wcm": return "application/vnd.ms-works";
                case ".wdb": return "application/vnd.ms-works";
                case ".wdp": return "image/vnd.ms-photo";
                case ".webarchive": return "application/x-safari-webarchive";
                case ".webtest": return "application/xml";
                case ".wiq": return "application/xml";
                case ".wiz": return "application/msword";
                case ".wks": return "application/vnd.ms-works";
                case ".WLMP": return "application/wlmoviemaker";
                case ".wlpginstall": return "application/x-wlpg-detect";
                case ".wlpginstall3": return "application/x-wlpg3-detect";
                case ".wm": return "video/x-ms-wm";
                case ".wma": return "audio/x-ms-wma";
                case ".wmd": return "application/x-ms-wmd";
                case ".WMD": return "application/x-ms-wmd";
                case ".wmf": return "application/x-msmetafile";
                case ".wml": return "text/vnd.wap.wml";
                case ".wmlc": return "application/vnd.wap.wmlc";
                case ".wmls": return "text/vnd.wap.wmlscript";
                case ".wmlsc": return "application/vnd.wap.wmlscriptc";
                case ".wmp": return "video/x-ms-wmp";
                case ".wmv": return "video/x-ms-wmv";
                case ".wmx": return "video/x-ms-wmx";
                case ".wmz": return "application/x-ms-wmz";
                case ".wpl": return "application/vnd.ms-wpl";
                case ".wps": return "application/vnd.ms-works";
                case ".wri": return "application/x-mswrite";
                case ".wrl": return "x-world/x-vrml";
                case ".wrz": return "x-world/x-vrml";
                case ".wsc": return "text/scriptlet";
                case ".wsdl": return "text/xml";
                case ".wvx": return "video/x-ms-wvx";
                case ".x": return "application/directx";
                case ".xaf": return "x-world/x-vrml";
                case ".xaml": return "application/xaml+xml";
                case ".xap": return "application/x-silverlight-app";
                case ".xbap": return "application/x-ms-xbap";
                case ".xbm": return "image/x-xbitmap";
                case ".xdr": return "text/plain";
                case ".xht": return "application/xhtml+xml";
                case ".xhtml": return "application/xhtml+xml";
                case ".xla": return "application/vnd.ms-excel";
                case ".xlam": return "application/vnd.ms-excel.addin.macroEnabled.12";
                case ".xlc": return "application/vnd.ms-excel";
                case ".xld": return "application/vnd.ms-excel";
                case ".xlk": return "application/vnd.ms-excel";
                case ".xll": return "application/vnd.ms-excel";
                case ".xlm": return "application/vnd.ms-excel";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlsb": return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                case ".xlsm": return "application/vnd.ms-excel.sheet.macroEnabled.12";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xlt": return "application/vnd.ms-excel";
                case ".xltm": return "application/vnd.ms-excel.template.macroEnabled.12";
                case ".xltx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case ".xlw": return "application/vnd.ms-excel";
                case ".xml": return "text/xml";
                case ".xmta": return "application/xml";
                case ".xof": return "x-world/x-vrml";
                case ".XOML": return "text/plain";
                case ".xpm": return "image/x-xpixmap";
                case ".xps": return "application/vnd.ms-xpsdocument";
                case ".xrm-ms": return "text/xml";
                case ".xsc": return "application/xml";
                case ".xsd": return "text/xml";
                case ".xsf": return "text/xml";
                case ".xsl": return "text/xml";
                case ".xslt": return "text/xml";
                case ".xsn": return "application/octet-stream";
                case ".xss": return "application/xml";
                case ".xtp": return "application/octet-stream";
                case ".xwd": return "image/x-xwindowdump";
                case ".z": return "application/x-compress";
                case ".zip": return "application/x-zip-compressed";

                #endregion

                default:
                    // if you have logging, log: "No mime type is registered for extension: " + extension);
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// Generates a random digit code
        /// </summary>
        /// <param name="length">Length string</param>
        /// <returns>Returns a applicationHost string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            Random random = new Random();

            string result = string.Empty;

            for (int index = 0; index < length; index++)
            {
                result = string.Concat(result, random.Next(10).ToString());
            }

            return result;
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length">Length string</param>
        /// <returns>Returns a applicationHost string</returns>
        public static string GenerateRandomString(int length)
        {
            Random random = new Random();

            char[] allowableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            StringBuilder result = new StringBuilder();

            for (int index = 0; index < length; index++)
            {
                result.Append(allowableChars[(int)(random.NextDouble() * allowableChars.Length)]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Trust level of AspNetHostingPermissionLevel
        /// </summary>
        private static AspNetHostingPermissionLevel? _trustLevel = null;

        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                // set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                // determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in
                        new AspNetHostingPermissionLevel[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; // we've set the highest permission we can
                    }
                    catch (SecurityException)
                    {
                        continue;
                    }
                }
            }

            return _trustLevel.Value;
        }

        /// <summary>
        /// Creates a new list of any type or anonymous types
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="itemOfType">Item type</param>
        /// <returns>Returns a new list</returns>
        public static List<T> MakeList<T>(T itemOfType)
        {
            List<T> newList = new List<T>();
            return newList;
        }

        /// <summary>
        /// Clones a element list
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="source">Source list to clone</param>
        /// <returns>Returns a cloned list</returns>
        public static IList<T> CloneList<T>(IList<T> source) where T : ICloneable
        {
            IList<T> clone = new List<T>(source.Count);

            foreach (T t in source)
            {
                clone.Add((T)t.Clone());
            }

            return clone;
        }

        /// <summary>
        /// Extracts a string from between a pair of delimiters. Only the first 
        /// instance is found.
        /// </summary>
        /// <param name="source">Input String to work on</param>
        /// <param name="beginDelim">Beginning delimiter</param>
        /// <param name="endDelim">ending delimiter</param>
        /// <param name="caseSensitive">Determines whether the search for delimiters is case sensitive</param>
        /// <param name="allowMissingEndDelimiter">A item indicating whether allow missing end delimiters</param>
        /// <param name="returnDelimiters">A item indicating whether must return delimiters</param>
        /// <returns>Extracted string or ""</returns>
        public static string ExtractString(string source, string beginDelim, string endDelim, bool caseSensitive = false, bool allowMissingEndDelimiter = false, bool returnDelimiters = false)
        {
            int at1, at2;

            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            if (caseSensitive)
            {
                at1 = source.IndexOf(beginDelim);
                if (at1 == -1)
                {
                    return string.Empty;
                }

                if (!returnDelimiters)
                {
                    at2 = source.IndexOf(endDelim, at1 + beginDelim.Length);
                }
                else
                {
                    at2 = source.IndexOf(endDelim, at1);
                }
            }
            else
            {
                at1 = source.IndexOf(beginDelim, 0, source.Length, StringComparison.OrdinalIgnoreCase);
                if (at1 == -1)
                {
                    return string.Empty;
                }

                if (!returnDelimiters)
                {
                    at2 = source.IndexOf(endDelim, at1 + beginDelim.Length, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    at2 = source.IndexOf(endDelim, at1, StringComparison.OrdinalIgnoreCase);
                }
            }

            if (allowMissingEndDelimiter && at2 == -1)
            {
                return source.Substring(at1 + beginDelim.Length);
            }

            if (at1 > -1 && at2 > 1)
            {
                if (!returnDelimiters)
                {
                    return source.Substring(at1 + beginDelim.Length, at2 - at1 - beginDelim.Length);
                }
                else
                {
                    return source.Substring(at1, at2 - at1 + endDelim.Length);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Adds the script to lock screen during postback
        /// </summary>
        /// <param name="page">Page where the JavaScript will be registered</param>
        public static void AddLockScreenScript(Page page)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered(page.GetType(), "skm_LockScreen_divs"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "skm_LockScreen_divs", @"<div id=""skm_LockBackground"" class=""LockOff""></div><div id=""skm_LockPane"" class=""LockOff""><div id=""skm_LockPaneText"">&nbsp;</div></div>", false);
            }
        }

        /// <summary>
        /// Gets the versioned name of any resource
        /// </summary>
        /// <param name="relativeUrl">Resource name</param>
        /// <param name="useLastModifiedDate">A item indicating whether must use last modified date</param>
        /// <returns>Returns a string</returns>
        public static string GetCacheAwareUrl(string relativeUrl, bool useLastModifiedDate)
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);

            string path = WebHelper.ResolveUrl(relativeUrl);

            if (context.IsDebuggingEnabled)
            {
                return path;
            }

            string physicalPath = WebHelper.MapPath(relativeUrl);
            if (!File.Exists(physicalPath))
            {
                return path;
            }

            string version;
            if (context.Cache[physicalPath] == null)
            {
                if (useLastModifiedDate)
                {
                    version = GetFileLastModifiedDate(context, physicalPath);
                }
                else
                {
                    version = GetMD5FileHash(context, physicalPath);
                }

                context.Cache.Insert(physicalPath, version, new CacheDependency(physicalPath));
            }
            else
            {
                version = context.Cache[physicalPath].ToString();
            }

            return string.Format("{0}?v={1}", path, version);
        }

        /// <summary>
        /// DateDiff in SQL style
        /// </summary>
        /// <param name="interval">Date interval</param>
        /// <param name="date1">Start date</param>
        /// <param name="date2">End date</param>
        /// <returns>Returns the difference</returns>
        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            TimeSpan differenceTimeSpan = date2 - date1;

            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return Fix(differenceTimeSpan.TotalDays) / 7;
                case DateInterval.Day:
                    return Fix(differenceTimeSpan.TotalDays);
                case DateInterval.Hour:
                    return Fix(differenceTimeSpan.TotalHours);
                case DateInterval.Minute:
                    return Fix(differenceTimeSpan.TotalMinutes);
                default:
                    return Fix(differenceTimeSpan.TotalSeconds);
            }
        } 

        /// <summary>
        /// Saves a image to physical path
        /// </summary>
        /// <param name="physicalPath">Physical path</param>
        /// <param name="imageContent">Image content</param>
        public static void SaveImageToDisk(string physicalPath, byte[] imageContent)
        {
            FileStream fileStream = new FileStream(physicalPath, FileMode.CreateNew);
            BinaryWriter writer = new BinaryWriter(fileStream);

            try
            {
                writer.Write(imageContent);
            }
            finally
            {
                fileStream.Close();
                writer.Close();
            }
        }

        public static T CloneObject<T>(T obj)
        {
            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();

            dcSer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;

            T newObject = (T)dcSer.ReadObject(memoryStream);
            return newObject;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the hash data from local file
        /// </summary>
        /// <returns>Returns the hash</returns>
        private static string LoadHash()
        {
            string result = "1.0.0.0";

            string filePath = HttpRuntime.AppDomainAppPath + "assethash.txt";

            if (!File.Exists(filePath))
            {
                FileStream fileStream = null;
                StreamWriter writer = null;

                try
                {
                    fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.None);

                    writer = new StreamWriter(fileStream);

                    fileStream = null;

                    writer.Write(result);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                    }

                    if (writer != null)
                    {
                        writer.Dispose();
                    }
                }
            }

            ICacheManager cacheManager = IoC.Resolve<ICacheManager>();

            object resultCached = cacheManager.Get(ResourceOptimizationHashCacheKey);

            if (resultCached != null)
            {
                return resultCached.ToString();
            }

            using (var sr = new StreamReader(filePath))
            {
                while (sr.Peek() >= 0)
                {
                    result = sr.ReadLine();
                    break;
                }
            }

            cacheManager.Add(ResourceOptimizationHashCacheKey, result);

            return result;
        }

        /// <summary>
        /// Get the MD5 file hash
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <param name="physicalPath">Physical path</param>
        /// <returns>Returns a hash string</returns>
        private static string GetMD5FileHash(HttpContextBase context, string physicalPath)
        {
            byte[] hash = MD5.Create().ComputeHash(File.ReadAllBytes(physicalPath));

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        /// <summary>
        /// Get the last modified date of any file
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <param name="physicalPath">Physical path</param>
        /// <returns>Returns a date with format yyyyMMddhhmmss</returns>
        private static string GetFileLastModifiedDate(HttpContextBase context, string physicalPath)
        {
            return new FileInfo(physicalPath).LastWriteTime.ToString("yyyyMMddhhmmss");
        }

        /// <summary>
        /// Fixes a number to long data type to find the date difference
        /// </summary>
        /// <param name="number">Input number</param>
        /// <returns>Returns the fixed number</returns>
        private static long Fix(double number)
        {
            if (number >= 0)
            {
                return (long)Math.Floor(number);
            }

            return (long)Math.Ceiling(number);
        }

        #endregion
    }
}