import React, { useEffect, useState } from "react";
import Table from "react-bootstrap/Table";
import { Link } from "react-router-dom";
import { getPosts } from "../../../Services/BlogRepository";
import Loading from "../../../Components/Loading";
// import { Tab } from "react-bootstrap";
import PostFilterPane from "../../../Components/Admin/PostFilterPane";

const Posts = ()=>{
  const [postList, setPostList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true);

  let k = '', p = 1, ps = 10;

  useEffect(() => {
    document.title = 'Danh sách bài viết';

    getPosts(k, ps, p).then(data => {
      if (data)
        setPostList(data.items);
      else
        setPostList([]);
      setIsVisibleLoading(false);
    })
  }, [k, p, ps])

  return (
    <>
      <h1>Danh sách bài viết</h1>
      <PostFilterPane />
      {isVisibleLoading ? <Loading /> :
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tiêu đề</th>
              <th>Tác giả</th>
              <th>Chủ đề</th>
              <th>Xuất bản</th>
            </tr>
          </thead>
          <tbody>
            {postList.length > 0 ? postList.map((item, index) =>
            <tr key={index}>
              <td>
                <Link 
                  to={`/admin/posts/edit/${item.id}`}
                  className='text-bold'>
                    {item.title}
                  </Link>
                  <p className='text-muted'>{item.shortDescription}</p>
              </td>
              <td>{item.author.fullName}</td>
              <td>{item.category.name}</td>
              <td>{item.published ? "Có" : "Không"}</td>
            </tr>
            ) :
              <tr>
                <td colSpan={4}>
                  <h4 className='text-danger text-center'>Không tìm thấy bài viết nào</h4>
                </td>
              </tr>}
          </tbody>
        </Table>
      }
    </>
  );
}

export default Posts;