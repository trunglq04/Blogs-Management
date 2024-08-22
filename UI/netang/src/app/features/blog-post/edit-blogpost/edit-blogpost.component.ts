import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MarkdownComponent } from 'ngx-markdown';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';
import { ImageSelectorComponent } from '../../../shared/components/image-selector/image-selector.component';
import { ImageService } from '../../../shared/components/image-selector/image.service';

@Component({
  selector: 'app-edit-blogpost',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MarkdownComponent,
    ImageSelectorComponent,
  ],
  templateUrl: './edit-blogpost.component.html',
  styleUrl: './edit-blogpost.component.css',
})
export class EditBlogpostComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model?: BlogPost;
  categories$?: Observable<Category[]>;
  selectedCategories?: string[];
  isImageSelectorVisible: boolean = false;

  routeSubscription?: Subscription;
  getBlogPostSubscription?: Subscription;
  updateBlogPostSubscription?: Subscription;
  deleteBlogPostSubscription?: Subscription;
  imageSelectSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private router: Router,
    private imageService: ImageService
  ) {}

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();

    this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        // Get BlogPost From API
        if (this.id) {
          this.getBlogPostSubscription = this.blogPostService
            .getBlogPostById(this.id)
            .subscribe({
              next: (response) => {
                this.model = response;
                this.selectedCategories = response.categories.map((x) => x.id);
              },
            });
        }
      },
    });

    this.imageSelectSubscription = this.imageService.selectedImage.subscribe({
      next: (response) => {
        if (this.model) {
          this.model.featuredImageUrl = response.url;
          this.isImageSelectorVisible = false;
        }
      },
    });
  }

  onFormSubmit(): void {
    // Convert this model to Request Object
    if (this.model && this.id) {
      var updateBlogPost: UpdateBlogPost = {
        title: this.model.title,
        shortDescription: this.model.shortDescription,
        content: this.model.content,
        featuredImageUrl: this.model.featuredImageUrl,
        urlHandle: this.model.urlHandle,
        author: this.model.author,
        publishedDate: this.model.publishedDate,
        isVisible: this.model.isVisible,
        categories: this.selectedCategories ?? [],
      };

      this.updateBlogPostSubscription = this.blogPostService
        .updateBlogPostById(this.id, updateBlogPost)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/blogposts');
          },
        });
    }
  }

  onDelete(): void {
    if (this.id) {
      // Call service and delete blogpost
      this.deleteBlogPostSubscription = this.blogPostService
        .deleteBlogPost(this.id)
        .subscribe({
          next: (response) => {
            this.router.navigateByUrl('/admin/blogposts');
          },
        });
    }
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getBlogPostSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
    this.deleteBlogPostSubscription?.unsubscribe();
    this.imageSelectSubscription?.unsubscribe();
  }
}
