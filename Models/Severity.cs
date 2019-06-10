/*
 *  Definition for the Severity Enumeration. 
 *  Used to distinguish tickets based on their
 *  priority.
 */


using System.ComponentModel.DataAnnotations;


/// <summary>
/// Enum 
/// <c>Severity</c> 
/// Enumeration for ticket severities. In future versions, this 
/// enumeration will replaced by a static class.
/// </summary>
public enum Severity
{
    [Display(Name = "Very Low")]
    VeryLow = 1,

    [Display(Name = "Low")]
    Low = 2,

    [Display(Name = "Medium")]
    Medium = 3,

    [Display(Name = "High")]
    High = 4,

    [Display(Name = "Very High")]
    VeryHigh = 5
}
