import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AddBlogPost } from '../models/add-blog-post.model';
import { FormsModule } from '@angular/forms';
import { BlogPostService } from '../services/blog-post.service';
import { MarkdownModule } from 'ngx-markdown';
import { CategoryService } from '../../category/services/category.service';
import { Observable } from 'rxjs';
import { Category } from '../../category/models/category.model';

@Component({
  selector: 'app-add-blogpost',
  standalone: true,
  imports: [RouterModule, FormsModule, DatePipe, MarkdownModule, CommonModule],
  templateUrl: './add-blogpost.component.html',
  styleUrl: './add-blogpost.component.css',
})
export class AddBlogpostComponent implements OnInit {
  model: AddBlogPost;
  categories$?: Observable<Category[]>;

  constructor(
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private router: Router
  ) {
    this.model = {
      title: '',
      shortDescription: '',
      content: '',
      featuredImageUrl: '',
      urlHandle: '',
      author: '',
      publishedDate: new Date(),
      isVisible: true,
      categories: []
    };
  }
  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();
  }

  onFormSubmit(): void {
    console.log(this.model);
    
    this.blogPostService.createBlogPost(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('admin/blogposts');
      },
    });
  }
}
