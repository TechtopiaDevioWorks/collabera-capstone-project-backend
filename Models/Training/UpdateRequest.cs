namespace WebApi.Models.Training;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
using CustomAnnotations;

public class UpdateRequest
{
    private string _name;
    private string _description;

    [CustomAnnotations.RequiredIfNot("end", null)]
    public DateTime? start { get; set; }

    [CustomAnnotations.RequiredIfNot("start", null)]
    public DateTime? end { get; set; }

    [Range(1, 1000)]
    public int? min_hours { get; set; }

    public byte? status_id { get; set; }

    public string name
    {
        get => _name;
        set => _name = replaceEmptyWithNull(value);
    }

    public string description
    {
        get => _description;
        set => _description = replaceEmptyWithNull(value);
    }


    private string replaceEmptyWithNull(string value)
    {
        // replace empty string with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }

}