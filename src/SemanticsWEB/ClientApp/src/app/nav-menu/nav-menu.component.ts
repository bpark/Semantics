import { Component } from '@angular/core';
import {RdfDataService} from "../graph/rdf-data.service";
import {GraphDataStateService} from "../graph/graph-data-state.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  searchText: string;

  constructor(private graphDataStateService: GraphDataStateService) {

  }

  onSubmit() {
    console.log('searchText: ', this.searchText);
    this.graphDataStateService.queryData("Literal", this.searchText);
  }
}
