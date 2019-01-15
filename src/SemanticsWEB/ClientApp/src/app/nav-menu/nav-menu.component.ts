import { Component } from '@angular/core';
import {RdfDataService} from "../graph/rdf-data.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  searchText: string;

  constructor(private rdfDataService: RdfDataService) {

  }

  onSubmit() {
    console.log('searchText: ', this.searchText);
  }
}
