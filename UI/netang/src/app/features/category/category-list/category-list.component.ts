import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css',
})
export class CategoryListComponent implements OnInit {
  // categories?: Category[];
  categories$?: Observable<Category[]>;
  totalCount?: number;
  pages: number[] = [];
  pageNumber = 1;
  pageSize = 3;

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.categoryService.getCategoryCount().subscribe({
      next: (value) => {
        this.totalCount = value;
        this.pages = new Array<number>(
          Math.ceil(this.totalCount / this.pageSize)
        )
          .fill(0)
          .map((_, i) => i + 1);

        this.getAllCategories();
      },
    });
  }

  getAllCategories() {
    this.categories$ = this.categoryService.getAllCategories(
      undefined,
      undefined,
      undefined,
      this.pageNumber,
      this.pageSize
    );
  }

  onSearch(query: string) {
    this.categories$ = this.categoryService.getAllCategories(query);
  }

  sort(sortBy: string, sortDirection: string) {
    this.categories$ = this.categoryService.getAllCategories(
      undefined,
      sortBy,
      sortDirection
    );
  }

  getPage(pageNumber: number) {
    if (pageNumber === this.pageNumber) {
      return;
    }

    this.pageNumber = pageNumber;
    this.getAllCategories();
  }

  getPreviousPage() {
    if (this.pageNumber - 1 < 1) {
      return;
    }
    this.pageNumber -= 1;
    this.getAllCategories();
  }

  getNextPage() {
    if (this.pageNumber + 1 > this.pages.length) {
      return;
    }
    this.pageNumber += 1;
    this.getAllCategories();
  }
}
