import "./App.css";
import Navbar from "./Components/Navbar";
import Sidebar from "./Components/Sidebar";
import Footer from "./Components/Footer";
import Layout from "./Pages/Layout";
import Index from "./Pages/Index";
import About from "./Pages/About";
import Contact from "./Pages/Contact";
import RSS from "./Pages/Rss";

import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import AdminLayout from "./Pages/Admin/Layout";
import * as AdminIndex from "./Pages/Admin/Index";
import Authors from "./Pages/Admin/Authors";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<Index />} />
          <Route path="blog" element={<Index />} />
          <Route path="blog/Contact" element={<Contact />} />
          <Route path="blog/About" element={<About />} />
          <Route path="blog/RSS" element={<RSS />} />
        </Route>
        <Route path="/admin" element={<AdminLayout />}>
          <Route path="/admin" element={<AdminIndex.default />} />
          <Route path="/admin/authors" element={<authors />} />
          <Route path="/admin/categories" element={<categories />} />
          <Route path="/admin/comments" element={<comments />} />
          <Route path="/admin/posts" element={<posts />} />
          <Route path="/admin/tags" element={<tags />} />
        </Route>
      </Routes>

      <Footer />
    </Router>
  );
}

export default App;
