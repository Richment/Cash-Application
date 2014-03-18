using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Security.Server; 

namespace LightSwitchApplication
{
	public partial class ApplicationDataService
	{

		partial void SortedQuery_PreprocessQuery(ref IQueryable<Artikelliste> query)
		{
			query = query.ToArray().OrderBy(n => n.Position).AsQueryable();
		}
	}
}
