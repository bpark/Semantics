import { Injectable } from '@angular/core';
import {Subject} from "rxjs";
import {Data, DataSet, Edge, Node} from "vis";
import {RdfDataService} from "./rdf-data.service";
import {VisNode} from "./data-model";

@Injectable({
  providedIn: 'root'
})
export class GraphDataStateService {

  private readonly graphData: Data;
  private graphDataSubject = new Subject<Data>();

  graphData$ = this.graphDataSubject.asObservable();

  constructor(private rdfDataService: RdfDataService) {
    this.graphData = {};
    this.graphData["nodes"] = new DataSet();
    this.graphData["edges"] = new DataSet();
  }

  public queryData(nodeType: string, resource: string): void {
    this.rdfDataService.get(nodeType, resource).subscribe(result => {
      console.log(result);

      let nodeData = this.graphData["nodes"] as DataSet<Node>;
      let edgeData = this.graphData["edges"] as DataSet<Edge>;

      const visNodes: VisNode[] = result.nodes.filter(node => nodeData.get(node.id) == null).map(node => {
        let color;
        if (node.nodeType != 'Uri') {
          color = {background: '#FF8800', border: '#FF8800'};
        } else {
          color = {background: '#2A9FD6', border: '#2A9FD6'};
        }
        return {
          id: node.id,
          label: node.label,
          nodeType: node.nodeType,
          color: color,
          value: 1
        };
      });

      console.log(edgeData.get([result.edges[0].from, result.edges[0].to]).filter(r => r != null));

      const edges = result.edges.filter(edge => edgeData.get(edge.id) == null);

      nodeData.add(visNodes);
      edgeData.add(edges);

      this.weightNodes();

      this.graphDataSubject.next(this.graphData);
    });
  }

  private weightNodes(): void {

    let nodeData = this.graphData["nodes"] as DataSet<Node>;
    let edgeData = this.graphData["edges"] as DataSet<Edge>;

    nodeData.forEach(node => {
      node.value = 0;
    });

    edgeData.forEach(edge => {
      const nodeFrom = nodeData.get(edge.from);
      if (nodeFrom != null) {
        nodeFrom.value++;
        nodeData.update(nodeFrom);
      }
      const nodeTo = nodeData.get(edge.to);
      if (nodeTo != null) {
        nodeTo.value++;
        nodeData.update(nodeTo);
      }

    });
  }
}
