using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.SuggestionServices
{
    public interface ISuggestionSystem
    {
        IDataResult<List<List<string>>> GetAprioriAlgorithm(List<List<string>> allPreferences);
    }
}
