import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BlogPostService } from '../services/blog-post.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-blogpost-list',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './blogpost-list.component.html',
  styleUrl: './blogpost-list.component.css'
})
export class BlogpostListComponent implements OnInit {

  blogPosts$?: Observable<BlogPost[]>

  constructor(private blogPostService: BlogPostService) {

  }

  ngOnInit(): void {
    // Get all blog posts from the API
    this.blogPosts$ = this.blogPostService.getAllBlogPosts();
  }

}
