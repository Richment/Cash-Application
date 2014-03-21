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
		partial void Artikelliste_Inserting(ArtikellisteItem entity)
		{
			entity.PositionIntern = entity.Position;
		}
		partial void Artikelliste_Updating(ArtikellisteItem entity)
		{ 
			entity.PositionIntern = entity.Position;
		}
	}
}
