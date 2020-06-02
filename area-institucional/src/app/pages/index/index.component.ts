import { Component, OnInit, Inject } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss'],
})

/**
 * Index-1 component
 */
export class IndexComponent implements OnInit {

  currentSection = 'home';

  constructor(@Inject(DOCUMENT) private document: Document, private modalService: NgbModal) { }

  ngOnInit(): void {
  }

  goToPlataforma()
  {
    this.document.location.href = 'https://app.mminvestimentos.com.br';
  }

  /**
   * Window scroll method
   */
  windowScroll() {
    const navbar = document.getElementById('navbar');
    if (document.body.scrollTop >= 50 || document.documentElement.scrollTop > 50) {
      navbar.classList.add('nav-sticky');
    } else {
      navbar.classList.remove('nav-sticky');
    }
  }

  /**
   * Section changed method
   * @param sectionId specify the current sectionID
   */
  onSectionChange(sectionId: string) {
    this.currentSection = sectionId;
  }

  /**
   * Toggle navbar
   */
  toggleMenu() {
    document.getElementById('navbarCollapse').classList.toggle('show');
  }

  /**
   * Login modal
   */
  loginModal(content) {
    this.modalService.open(content, { centered: true });
  }

  /**
   * Register modal
   * @param registercontent content
   */
  registerModal(registercontent) {
    this.modalService.open(registercontent, { centered: true });
  }
}
