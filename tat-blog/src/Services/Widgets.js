// import axios from "axios";

// export async function getCategories(){
//   try {
//     const response = await
//     axios.get('https://localhost:7299/api/categories');
//       const data = response.data;
//       if (data.isSuccess)
//         return data.result;
//       else
//         return null;
//   } catch (error) {
//       console.log('Error', error.message);
//     return null;
//   }
// }

import { get_api } from "./Methods";

export function getCategories() {
  return get_api(`https://localhost:7299/api/categories`);
}