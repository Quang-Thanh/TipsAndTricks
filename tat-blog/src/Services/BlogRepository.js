//import axios from "axios";
import { get_api } from './Methods';

export function getPosts(
  keyword = '',
  pageSize = 10,
  pageNumber = 1,
  sortColumn = '',
  sortOrder = '') {
    return
    get_api(`https:localhost:7299/api/posts?keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}
    &SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
  }

  export function getAuthors(name = '',
    pageSize = 10,
    pageNumber = 1,
    sortColumn = '',
    sortOrder = '') {
      return
      get_api(`https:localhost:7299/api/authors?name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}
    &SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
    }

    export function getFilter() {
      return get_api('https://localhost:7229/api/posts/get-filter');
    }

// export async function getPosts(
//   keyword = "",
//   pageSize = 10,
//   pageNumber = 1,
//   sortColumn = "",
//   sortOrder = ""
// ) {
//   try {
//     const response = await axios.get(
//       `https://localhost:7299/api/posts?PageSize=${pageSize}&PageNumber=${pageNumber}`
//     );
//     //axios.get(`https://localhost:7229/api/posts?keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
//     const data = response.data;
//     if (data.isSuccess) return data.result;
//     else return null;
//   } catch (error) {
//     console.log("Error", error.message);
//     return null;
//   }
// }
