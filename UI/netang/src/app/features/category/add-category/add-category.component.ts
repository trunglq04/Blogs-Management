import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms'; // Import FormsModule

import { AddCategoryRequest } from '../models/add-category-request.model';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css',
})
export class AddCategoryComponent implements OnDestroy {
  model: AddCategoryRequest;
  private addCategorySubscription?: Subscription;

  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {
    this.model = {
      name: '',
      urlHandle: '',
    };
  }

  onFormSubmit() {
    this.addCategorySubscription = this.categoryService
      .addCategory(this.model)
      .subscribe({
        next: (response) => {
          console.log('This was successful!', response);
          this.router.navigateByUrl('/admin/categories');
        },
        error: (err) => {
          console.log(err);
        },
      });
  }

  ngOnDestroy(): void {
    this.addCategorySubscription?.unsubscribe();
  }
}
