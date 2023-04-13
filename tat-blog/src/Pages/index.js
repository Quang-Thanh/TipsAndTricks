import React, { useEffect, useState } from "react";
import PostItem from "../Components/PostItem";
import Pager from "../Components/Pager";
import { getPosts } from "../Services/BlogRepository";
import { useQuery } from "../Utils/Utils";


const Index = () => {
  const [postList, setPostList] = useState([]);
  const [metadata, setMetadata] = useState({});

  let query = useQuery(),
    keyword = query.get("k") ?? "",
    p = query.get("p") ?? 1,
    ps = query.get("ps") ?? 5;

  useEffect(() => {
    document.title = "Trang chủ";

    getPosts(keyword, ps, p).then((data) => {
      if (data) {
        setPostList(data.items);
        setMetadata(data.metadata);
      } else setPostList([]);
    });
  }, [keyword, ps, p]);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [postList]);

  if (postList.length > 0) {
    return (
      <div className="p-4">
        {postList.map((item, index) => {
          return <PostItem postItem={item} key={index} />;
        })}

        {/* <Pager postquery={{ 'keyword' : k }} metadata={metadata} /> */}
        <Pager postquery={{ keyword }} metadata={metadata} />
      </div>
    );
  } else {
    // syntax nhu nay nha
    return <></>;
  }

  // else return (
  //   <></>
  // );
};

export default Index;
