using System.Text.RegularExpressions;

namespace RoutingExample.CustomConstraints
{
    // Eg: sales-report-with-custom-constraint/2020/apr
    public class MonthsCustomConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey))
            {
                return false; // not a match
            }

            Regex regex = new Regex("^(apr|jul|oct|jan)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            return regex.IsMatch(monthValue);
        }
    }
}
