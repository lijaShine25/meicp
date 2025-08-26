<%@ WebHandler Language="C#" Class="FileHandler" %>

using System;
using System.Web;

public class FileHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        // Get the file name from the query string or any other source
        string fileName = context.Request.QueryString["fileName"];

        // Set the content type based on the file extension
        context.Response.ContentType = MimeMapping.GetMimeMapping(fileName);

        // Set the content-disposition header to open in the browser
        context.Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);

        // Read the file and write it to the response stream
        string filePath = context.Server.MapPath("Documents/" + fileName);
        context.Response.WriteFile(filePath);
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
