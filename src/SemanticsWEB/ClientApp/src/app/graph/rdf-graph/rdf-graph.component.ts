import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Data, DataSet, Edge, Network, Node, Options} from 'vis';
import {VisNode} from "../data-model";
import {Observable, Subject} from 'rxjs';
import {GraphDataStateService} from "../graph-data-state.service";

@Component({
  selector: 'app-rdf-graph',
  templateUrl: './rdf-graph.component.html',
  styleUrls: ['./rdf-graph.component.css']
})
export class RdfGraphComponent implements OnInit {

  @ViewChild('net') set networkElementRef(networkElementRef: ElementRef) {
    if (networkElementRef != undefined) {
      this.createNetwork(networkElementRef);
    }
  }

  graphData: Data;
  network: Network;

  constructor(private graphDataStateService: GraphDataStateService) {}

  ngOnInit(): void {

    this.graphData = {};
    this.graphData["nodes"] = new DataSet();
    this.graphData["edges"] = new DataSet();

    this.graphDataStateService.graphData$.subscribe(graphData => {
      let nodeData = this.graphData["nodes"] as DataSet<Node>;
      let edgeData = this.graphData["edges"] as DataSet<Edge>;

      let newNodeData = graphData["nodes"] as DataSet<Node>;
      let newEdgeData = graphData["edges"] as DataSet<Edge>;

      nodeData.update(newNodeData.get());
      edgeData.update(newEdgeData.get());
    });

    this.graphDataStateService.queryData('Uri', 'permid:1-1003939166');
  }

  private createNetwork(networkElementRef: ElementRef): void {
    if (!this.network) {
      this.network = new Network(networkElementRef.nativeElement, this.graphData, this.createOptions());

      let gData = this.graphData;

      Observable.create((subject: Subject<VisNode>) => {

        this.network.on("click", function (params) {
          const selectedId = this.getNodeAt(params.pointer.DOM);
          let nodeData = gData['nodes'] as DataSet<Node>;
          nodeData.forEach(node => {
            if (node.id === selectedId) {
              let visNode = node as VisNode;
              subject.next(visNode);
            }
          });
        });
      }).subscribe(result => {
        console.log(result);
        this.graphDataStateService.queryData(result.nodeType, result.label);
      });


    }
  }

  private createOptions(): Options {
    return {
      edges: {
        //smooth: {
        //  forceDirection: "none"
        //},
        //font: {align: 'top'}
        font: {
          color: '#eeeeee',
          strokeWidth: 0
        }
      },
      physics: {
        barnesHut: {
          gravitationalConstant: -10200,
          springLength: 85
        },
        minVelocity: 0.75
      },
      nodes: {
        shape: 'dot',
        font: {
          color: '#eeeeee'
        }
        /*scaling: {
          label: {
            min: 8,
            max: 20
          }
        }*/
      },
      width: (window.innerWidth - 0) + "px",
      height: (window.innerHeight - 0) + "px"
    };
  }

}
