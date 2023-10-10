using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;
public interface IRepositoryManager {
    /*
     * The output of some APis may be a combination 
     * of data from several sources,
     * in this case, we keep instances 
     * from each repository and obtain 
     * the desired data by combining them,
     * for example, it may require a 
     * combination of 5 or 6 models in our logic. 
     */
    IEmployeeRepository Employee { get; }
    ICompanyRepository Company { get; }
    void Save();
}

